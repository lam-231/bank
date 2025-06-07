# BankProject

## Огляд функціоналу

BankProject — це ASP.NET Core MVC-додаток для банківських операцій, що підтримує три основні режими:

1. **Мобільна версія**  
   - Реєстрація та вхід користувача (Email + Phone + Address + Age + 4-значний PIN).  
   - Генерація унікальної 12-значної картки з CVV-кодом.  
   - Перегляд балансу та профілю.  
   - Керування контактами (додавання, видалення; кожен контакт має ім’я та номер картки, перевірку наявності картки).  
   - Переказ коштів між картками (за введеним номером або через список контактів).  

2. **Режим банкомату (ATM)**  
   - Вхід лише за номером картки, далі запит 4-значного PIN (пароля).  
   - Головний екран банкомату: показ поточного балансу користувача та «Готівка в банкоматі».  
   - Депозит коштів (максимум 5000 за раз): додає гроші на баланс користувача та у загальний запас банкомату.  
   - Зняття коштів: перевірка, чи користувач має достатньо грошей, а також чи в банкоматі є достатньо готівки; зменшення балансу й загального запасу.  

3. **Режим адміністратора**  
   - Панель адміністратора (`/Admin/Index`):  
     - Показ загальної кількості готівки в банкоматі.  
     - Форма «Додати готівку» (будь-яка сума) для поповнення запасу банкомату.  
     - Перелік усіх користувачів (Name, Surname, CardNumber) із можливістю перегляду деталізованої інформації.  
   - Детальна інформація про користувача (`/Admin/Details/{id}`):  
     - Усі поля користувача (Name, Surname, Phone, Email, Address, Age) та зв’язаної картки (CardNumber, CVV, Balance).  

Усі три режими пов’язані між собою через спільні сервіси, репозиторії та модель даних (Entity Framework Core + SQL Server).

---

## Запуск
В хедері є кнопки для перемикання між режимами Банкомат/Адмін/Мобільна версія

### Опис режиму мобільної версіх(телефону)
Після запуску з'являється вікно входу, в режимі телефону. Потрібно заповнити всі поля і натиснути увійти(можна подивитись людей через панель адміна).
Якщо користувачі відсутні, можна натиснути на 'Зареєструватись'. Після входу буде показано всі данні користувача, доступні кнопки "переказ коштів", "контакти" та "вихід".
Вихід просто поверне на стартове вікно телефону. В переказі коштів потрібно вказати суму та карту, але також можна вибрати контакт, якщо він присутній
і поле карти заповниться автоматично. У вікні контактів можна подивистись існуючі контакти або перейти до додавання нових. При додаванні можна вказати довільне ім'я
але номер карти потрібно вводити існуючий в бд, якщо буде введена неіснуюча - програма повідомить про це і запропонує ввести ще раз.

### Опис режиму Банкомату
В цьому відсутня реєстрація. Тут відразу доступний лише вхід. Спочатку потрібно ввести номер карти, якщо номер правильний, можна перейти до вводу PIN-коду.
На головній сторінці виведене привітання, вказані баланс, номер карти та кількість готівки в банкоматі(в справжніх це відсутнє, але так легше орієнтуватись).
Присутні кнопки, які переводять в режим "Зняти кошти" або "Покласти кошти", покласти можна гроші без перевірки на наявність їх десь, лише перевірка на 
максимальну кількість за раз(5000). Знімати ж можна не довільну суму, тут йде перевірка на достатність готівки на карті та в банкоматі.

### Опис режиму адміністратора
Цей режим є більш абстрактним, він потрібен більш для керування проектом. В ньому є можливість перегляду інформації про користувачів, а також додавання
готіки в банкомат, що умовна можна назвати імітацією роботи інкасаторів.

---
# Programming Principles

Нижче перелік принципів програмування, дотриманих у цьому проєкті. 

## 1 Single Responsibility Principle (SRP)

Кожен клас відповідає лише за одну відповідальність:

- **[UserRepository](./BankProject/Repositories/UserRepository.cs)** — лише доступ до таблиці Users.
- **[CardService](./BankProject/Services/CardService.cs)** — лише бізнес-логіка, пов’язана з картками (перекази).
- **[ATMController](./BankProject/Controllers/ATMController.cs)** — лише обробка HTTP-запитів для ATM.

Завдяки цьому класи простіші, їх легше тестувати та підтримувати.

## 2 Open/Closed Principle (OCP)

Класи відкриті для розширення, але закриті для модифікації.

Наприклад, щоб додати нові правила валідації для переказу, можна створити новий сервіс, який реалізує той самий інтерфейс [ICardService](./BankProject/Services/Interfaces/ICardService.cs), без зміни існуючого коду.

## 3 Liskov Substitution Principle (LSP)

Будь-яка реалізація інтерфейсу може замінити базову без порушення логіки.

У тестах можна підставити mock-реалізацію [ICardRepository](./BankProject/Repositories/Interfaces/ICardRepository.cs) у контролери, і логіка програми не постраждає.

## 4 Interface Segregation Principle (ISP)

Інтерфейси розбиті за зонами відповідальності:

- **[IUserRepository](./BankProject/Repositories/Interfaces/IUserRepository.cs)** — дефінії для роботи з User.
- **[ICardRepository](./BankProject/Repositories/Interfaces/ICardRepository.cs)** — дефінії для роботи з Card.
- **[IContactRepository](./BankProject/Repositories/Interfaces/IContactRepository.cs)** — дефінії для роботи з Contact.
- **[IATMRepository](./BankProject/Repositories/Interfaces/IATMRepository.cs)** — визначає тільки роботу з ATM (TotalCash).

Кожен клієнт залежить лише від тих методів, які йому потрібні.

## 5 Dependency Inversion Principle (DIP)

Контролери та сервіси залежать від абстракцій (інтерфейсів), а не від конкретних реалізацій.

Приклад:

```csharp
public class MobileController
{
    private readonly ICardService _cardService;
    private readonly IContactService _contactService;
    public MobileController(ICardService cardService, IContactService contactService)
    {
        _cardService = cardService;
        _contactService = contactService;
    }
    // ...
}
```
Інтерфейси реєструються в [Program.cs](./BankProject/Program.cs) → факти реалізацій підміняються DI-контейнером.

## 6 DRY (Don't Repeat Yourself)

Повторювану логіку винесено у загальні частини:

- Блоки перевірки сесії користувача у контролерах (`if (userId == null) return RedirectToAction("Login")`).
- Блоки відображення TempData-повідомлень у [_Layout.cshtml](./BankProject/Views/Shared/_Layout.cshtml).
- Моделі ViewModel з атрибутами валідації замість дублювання перевірок у контролерах.

## 7 KISS (Keep It Simple, Stupid)

Прості рішення без зайвих складнощів:

- Автозаповнення номера картки через  
  ```html
  <select asp-for="DestinationCardNumber" asp-items="Model.AvailableContacts" />
  ``` 
без додаткового JS (Варіант B).
- Логіка депозиту/зняття здійснена прямо в контролері без складних станів чи запитів.

## 8 Separation of Concerns

- Чіткий поділ між трьома шарами:
  - Data Access Layer (репозиторії: Repositories/*.cs).
  - Business Logic Layer (сервіси: Services/*.cs).
  - Presentation Layer (контролери та Views: Controllers/*.cs та Views/*/*.cshtml).
- Завдяки цьому зміни в одному шарі не впливають на інші без мінімального переписування.

## 9 YAGNI (You Aren’t Gonna Need It)

- Під час розробки код регулярно рефакторився:
  - Винесення великих методів у менші.
  - Створення нових ViewModel для уникнення передачі доменних моделей у Views.
  - Видалення зайвих методів після зміни вимог (наприклад, видалено CVV-перевірку з ATM).

---

# Design Patterns

У проєкті застосовано декілька патернів проєктування:

## 1 Repository Pattern

- **Опис**: Ізоляція доступу до джерела даних за допомогою інтерфейсів і класів-репозиторіїв.
- **Файли**:
  - [Repositories/UserRepository](./BankProject/Repositories/UserRepository.cs)
  - [Repositories/CardRepository](./BankProject/Repositories/CardRepository.cs)
  - [Repositories/ContactRepository](./BankProject/Repositories/ContactRepository.cs)
  - [Repositories/ATMRepository](./BankProject/Repositories/ATMRepository.cs)
- **Відповідні інтерфейси**:
  - [IUserRepository](./BankProject/Repositories/Interfaces/IUserRepository.cs)
  - [ICardRepository](./BankProject/Repositories/Interfaces/ICardRepository.cs)
  - [IContactRepository](./BankProject/Repositories/Interfaces/IContactRepository.cs)
  - [IATMRepository](./BankProject/Repositories/Interfaces/IATMRepository.cs)
- **Навіщо**:
  - Забезпечує єдину точку доступу до моделі.
  - Полегшує тестування (можна замінити реальний репозиторій mock’ом).
  - 
## 2 Service Layer (Facade)

- **Опис**: Групування бізнес-логіки в сервіси, що «маскують» складні операції за простими інтерфейсами.
- **Файли**:
  - [Services/UserService](./BankProject/Services/UserService.cs)
  - [Services/CardService](./BankProject/Services/CardService.cs)
  - [Services/ContactService](./BankProject/Services/ContactService.cs)
  - [Services/ATMService](./BankProject/Services/ATMService.cs)
- **Інтерфейси**:
  - [IUserService](./BankProject/Services/Interfaces/IUserService.cs)
  - [ICardService](./BankProject/Services/Interfaces/ICardService.cs)
  - [IContactService](./BankProject/Services/Interfaces/IContactService.cs)
  - [IATMService](./BankProject/Services/Interfaces/IATMService.cs)
- **Навіщо**:
  - Забезпечує єдину точку для складної бізнес-логіки (реєстрація, переказ, депозит, зняття).
  - Робить контролери «тоншими», перекладаючи більшу частину логіки на сервіси.

## 3 Dependency Injection (DI)

- **Опис**: Ін’єкція залежностей через конструктор.
- **Файл**:
  - [Program.cs](./BankProject/Program.cs) (реєстрація інтерфейсів і реалізацій)
- **Навіщо**:
  - Забезпечує слабку зв’язність компонентів.
  - Спрощує заміну реалізацій у тестах (наприклад, мок-репозиторії).

## 4 MVC (Model-View-Controller)

- **Опис**: Поділ на три шари: Модель, Вид, Контролер.
- **Файли**:
  - **Models**:
    - [Models/User.cs](./BankProject/Models/User.cs)
    - [Models/Card.cs](./BankProject/Models/Card.cs)
    - [Models/Contact.cs](./BankProject/Models/Contact.cs)
    - [Models/ATM.cs](./BankProject/Models/ATM.cs)
  - **ViewModels**:
    - [ViewModels/*.cs](./BankProject/ViewModels)
  - **Controllers**:
    - [Controllers/AccountController.cs](./BankProject/Controllers/AccountController.cs)
    - [Controllers/MobileController.cs](./BankProject/Controllers/MobileController.cs)
    - [Controllers/ATMController.cs](./BankProject/Controllers/ATMController.cs)
    - [Controllers/AdminController.cs](./BankProject/Controllers/AdminController.cs)
    - [Controllers/ContactsController.cs](./BankProject/Controllers/ContactsController.cs)
  - **Views**:
    - [Views](./BankProject/Views)
- **Навіщо**:
  - Розділяє відповідальність між обробкою запитів, логікою та відображенням.

## 5 ViewModel Pattern

- **Опис**: Використання спеціалізованих моделей для Views.
- **Файли**:
  - [ViewModels/TransferViewModel.cs](./BankProject/ViewModels/TransferViewModel.cs)
  - [ViewModels/ATMDepositViewModel.cs](./BankProject/ViewModels/ATMDepositViewModel.cs)
  - [ViewModels/ATMWithdrawViewModel.cs](./BankProject/ViewModels/ATMWithdrawViewModel.cs)
  - [ViewModels/AdminIndexViewModel.cs](./BankProject/ViewModels/AdminIndexViewModel.cs)
  - [ViewModels/AdminAddCashViewModel.cs](./BankProject/ViewModels/AdminAddCashViewModel.cs)
- **Навіщо**:
  - Забезпечує лише необхідні поля для форми, разом із атрибутами валідації.
  - Уникнення передачі доменних моделей безпосередньо у Views.

---

# Refactoring Techniques

Під час розробки програми було застосовано такі техніки рефакторингу:

## 1 Extract Class / Extract Interface

- Винесення логіки доступу до даних у окремі репозиторії (`UserRepository`, `CardRepository`, `ContactRepository`, `ATMRepository` [Репозиторії](./BankProject/Repositories)) та відповідні інтерфейси (`IUserRepository`, `ICardRepository`, `IContactRepository`, `IATMRepository` [Інтерфейси](./BankProject/Repositories/Interfaces)). 
- Винесення бізнес-логіки у сервіси (`UserService`, `CardService`, `ContactService`, `ATMService`[Сервіси](./BankProject/Services)) із відповідними інтерфейсами.

## 2 Extract Method

- У контролерах винесено отримання `userId` з сесії та логіку перенаправлення на сторінку вхід/реєстрація у приватні методи, щоб уникнути дублювання.

## 3 Rename

- Перейменування методів і змінних для кращої зрозумілості:
  - Наприклад, `GetByCardNumberAsync` (замість менш інформативного `FindCard`),
  - `SessionKeyUserId` (замість рядка `"UserId"`),
  - `ATMService.AddCashAsync` (замість `AddMoney`).

## 4 Introduce ViewModel

- Для кожної форми створено окремі ViewModel-і з атрибутами валідації (`DataAnnotations`), замість безпосереднього використання доменних класів у Views.

## 5 Simplify Conditional Expressions

- Логіку валідації (депозит/зняття) оптимізовано, розбивши складні умови на прості перевірки (`if … else`), іноді використовуючи «guard clauses».

## 6 Remove Dead Code / Inaccessible Code

- Після зміни вимог щодо перевірки CVV в ATM було видалено відповідні частини коду, які більше не використовуються.

## 7 Move Method / Move Class

- Розділено класи на відповідні папки:
  - Моделі (`Models`),
  - Репозиторії (`Repositories`),
  - Сервіси (`Services`),
  - Контролери (`Controllers`),
  - ViewModel-і (`ViewModels`).

## 8 Introduce Parameter Object

- У діях контролерів POST замість багатьох окремих параметрів передається один об’єкт ViewModel (наприклад, `ATMDepositViewModel model`).

## 9 Consolidate Duplicate Conditional Fragments

- Відображення повідомлень про успіх/помилки (`TempData["SuccessMessage"]`, `TempData["ErrorMessage"]`) винесено у `_Layout.cshtml`, замість дублювання у кожному окремому View.

## 10 Safety-Guard Clauses

- У початку дій контролерів — рання перевірка 
  ```csharp
  if (userId == null)
      return RedirectToAction("Login");

щоб уникнути глибокого вкладення умов.

