using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers
{
    public class VoiceMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly AppSettings _appSetting;
        private readonly IFileHandler _audioFileHandler;
        private readonly IStorage _memoryStorage;

        public VoiceMessageController(AppSettings appSettings, ITelegramBotClient telegramBotClient, IFileHandler fileHandler, IStorage memoryStorage)
        {
            _appSetting = appSettings;
            _telegramClient = telegramBotClient;
            _audioFileHandler = fileHandler;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Download(fileId, ct);

            string userLanguageCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode; // Здесь получим язык из сессии пользователя
            var result = _audioFileHandler.Process(userLanguageCode); // Запустим обработку
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);
        }

    }
}
