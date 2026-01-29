using System;
using System.Threading.Tasks;

using BusinessLogic;

namespace TelegramInteraction.Chat;

public class HardCodedPollRepository : IScheduledPollRepository
{
    public Task CreateAsync(ScheduledPollDbo poll)
    {
        throw new NotImplementedException();
    }

    public Task<ScheduledPollDbo[]> FindAsync(long chatId)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(ScheduledPollDbo poll)
    {
        throw new NotImplementedException();
    }

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
                            Options = new[] { "Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно", },
                        },
                    new()
                        {
                            Id = Guid.Parse("476d6ac2-245f-4edd-baa9-f0b48e63b300"),
                            ChatId = -1001286172835,
                            Schedule = "0 5 * * 1,3",
                            Name = "Йога у Лены завтра",
                            Options = new[] { "Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно", },
                        },
                    new()
                        {
                            Id = Guid.Parse("1bdac6c8-6096-4ba2-8c0c-addf865cdbbd"),
                            ChatId = -389442739,
                            Schedule = "0 5 * * 0,2,4",
                            Name = "Йога на НВ завтра",
                            Options = new[] { "Пойду в зал", "Пойду онлайн", "Не пойду", "Возможно", },
                        },
                    new()
                        {
                            Id = Guid.Parse("bd0c55e8-337c-41ee-a82c-fd60070b746d"),
                            ChatId = -1001290388650,
                            Schedule = "0 5 * * 2,4",
                            Name = "Запись на тренировку",
                            Options = new[] { "Я приду в зал", "Я буду онлайн", "Я не буду", },
                        },
                    new()
                        {
                            Id = Guid.Parse("2b9d03f5-0545-4551-9620-77bbcd71c929"),
                            ChatId = -1001332112170,
                            Schedule = "0 5 * * 2,4",
                            Name = "Тренировка завтра",
                            Options = new[] { "Зал", "Онлайн", "Мне только посмотреть", "Не приду", },
                        },
                    new()
                        {
                            Id = Guid.Parse("1e9f8a43-c162-478f-88f9-cf392ba71a24"),
                            ChatId = -1001332112170,
                            Schedule = "30 5 * * 5",
                            Name = "Тренировка в пн",
                            Options = new[] { "Зал", "Онлайн", "Мне только посмотреть", "Не приду", },
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
                            Id = Guid.Parse("abf38610-4d34-4dc9-8387-20913fdc58b7"),
                            ChatId = -1001860361407,
                            Schedule = "0 14 * * 1-5",
                            Name = "Кто где завтра работает?",
                            Options = new[] { "НГ", "НР", "Не работаю", "Любой другой вариант", },
                        },
                    new()
                        {
                            Id = Guid.Parse("7ab44aa3-1740-46f7-a3f8-dec54cbba925"),
                            ChatId = -1001893926406,
                            Schedule = "0 11 * * 1,3,5",
                            Name = "Играешь сегодня в бадминтон?",
                            Options = new[] { "Да", "Нет", },
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
                }
        );
    }

    public Task<ScheduledPollDbo> ReadAsync(Guid pollId)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(ScheduledPollDbo[] polls)
    {
        throw new NotImplementedException();
    }

    public Task<ScheduledPollDbo[]> ReadAllAsync()
    {
        throw new NotImplementedException();
    }
}