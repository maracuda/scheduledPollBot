using System;
using System.Threading.Tasks;

using BusinessLogic;

namespace TelegramInteraction.Chat;

public class HardCodedPollRepository : IScheduledPollRepository
{
    public Task<ScheduledPollDbo[]> FindNotDisabledAsync()
    {
        return Task.FromResult(new ScheduledPollDbo[]
                {
                    new()
                        {
                            Id = Guid.Parse("8dcf5f82-9562-4ba2-8ede-306c9a7b801f"),
                            ChatId = -1001318882712,
                            Schedule = "0 5 * * 1,4",
                            Name = "Йога у Саши завтра",
                            Options = new[] { "Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно" },
                        },
                    new()
                        {
                            Id = Guid.Parse("476d6ac2-245f-4edd-baa9-f0b48e63b300"),
                            ChatId = -1001286172835,
                            Schedule = "0 5 * * 1,3",
                            Name = "Йога у Лены завтра",
                            Options = new[] { "Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно" },
                        },
                    new()
                        {
                            Id = Guid.Parse("1bdac6c8-6096-4ba2-8c0c-addf865cdbbd"),
                            ChatId = -389442739,
                            Schedule = "0 5 * * 0,2,4",
                            Name = "Йога на НВ завтра",
                            Options = new[] { "Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно" },
                        },
                    new()
                        {
                            Id = Guid.Parse("9f327ea5-46b0-4cfd-9a1a-444ee44026b0"),
                            ChatId = -1001367822922,
                            Schedule = "0 3 * * 1-5",
                            Name = "Режим работы команды Маркет🤩",
                            Options = new[]
                                {
                                    "Ура, работаю в офисе🚀💃",
                                    "Работаю удаленно👩‍💻🧑‍💻",
                                    "Болею, энергосберегающий режим",
                                    "Другое😎",
                                },
                        },
                    new()
                        {
                            Id = Guid.Parse("7C56D955-3D2E-4181-BE8C-7CD3342C2651"),
                            ChatId = -855034942,
                            Schedule = "30 19 * * 0-4",
                            Name = "Где ты завтра?",
                            Options = new[]
                                {
                                    "Офис на \"Народной воли\"",
                                    "Офис на \"Широкой речке\"",
                                    "Другой офис",
                                    "Дома",
                                    "я в командировке",
                                    "у меня отпуск",
                                    "Болею :(",
                                },
                        },
                    new()
                        {
                            Id = Guid.Parse("abf38610-4d34-4dc9-8387-20913fdc58b7"),
                            ChatId = -1001860361407,
                            Schedule = "0 14 * * 1-5",
                            Name = "Кто где завтра работает?",
                            Options = new[] { "НГ", "НР", "Не работаю", "Любой другой вариант" },
                        },
                    new()
                        {
                            Id = Guid.Parse("a8665912-a372-47b5-8d99-ec64b71c6d2c"),
                            ChatId = -1001505366463,
                            Schedule = "0 6 * * 1-5",
                            Name = "Где работаешь сегодня?",
                            Options = new[]
                                {
                                    "🏢 Офис",
                                    "🏠 Дом, согласно утвержденного графика",
                                    "🥺 Дом, форс-мажор ",
                                    "🏖️ Отпуск",
                                    "🚀 Прочее",
                                },
                        },
                    new()
                        {
                            Id = Guid.Parse("540cc6b2-f6a5-430d-aba5-9b6e1a120cf5"),
                            ChatId = -773945108,
                            Schedule = "0 9 * * 1",
                            Name = "Играем завтра?",
                            Options = new[] { "Да", "Нет" },
                        },
                    new()
                        {
                            Id = Guid.Parse("2631f1a9-46d5-4ecb-8b47-0c764cec3885"),
                            ChatId = -773945108,
                            Schedule = "0 9 * * 3",
                            Name = "Играем завтра?",
                            Options = new[] { "Да", "Нет" },
                        },
                    new()
                        {
                            Id = Guid.Parse("82272953-138a-402c-9e1a-652bd1f13cb2"),
                            ChatId = -1001740754628,
                            Schedule = "0 2 * * 1",
                            Name = "Сегодня тренируешься?",
                            Options = new[] { "Да", "Нет", "Ответы" },
                        },
                    new()
                        {
                            Id = Guid.Parse("57f0847f-a05d-4fea-8832-4a9c190194c3"),
                            ChatId = -1001332112170,
                            Schedule = "0 5 * * 2,4",
                            Name = "Тренировка завтра",
                            Options = new[] { "Зал", "Онлайн", "Мне только посмотреть", "Не приду" },
                        },
                    new()
                        {
                            Id = Guid.Parse("ebbdc411-b387-450f-a823-a0b75fe4cefa"),
                            ChatId = -1001332112170,
                            Schedule = "30 5 * * 5",
                            Name = "Тренировка в пн",
                            Options = new[] { "Зал", "Онлайн", "Мне только посмотреть", "Не приду" },
                        },
                    new()
                        {
                            Id = Guid.Parse("b88e466f-9d3f-4fff-b485-dbac39feeaea"),
                            ChatId = -1001290388650,
                            Schedule = "0 5 * * 2,4",
                            Name = "Запись на тренировку",
                            Options = new[] { "Я приду в зал", "Я буду онлайн", "Я пропущу" },
                        },
                    new()
                        {
                            Id = Guid.Parse("85309b31-17e5-42e0-94e4-0c3fc23f2c62"),
                            ChatId = -1001946458560,
                            Schedule = "0 11 * * 1-5",
                            Name = "Кушац!",
                            Options = new[] { "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "Позже" },
                        },
                    new()
                        {
                            Id = Guid.Parse("30A1A427-40EE-4D18-98CD-0EC8766AAE3E"),
                            ChatId = -1001450404373,
                            Schedule = "0 14 * * 0",
                            Name = "Волейбол Вт 19.00-20.30",
                            Options = new[] { "+", "-" },
                        },
                }
        );
    }
}