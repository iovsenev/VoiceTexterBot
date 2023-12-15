using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using VoiceTexterBot.Configuration;
using FFMpegCore;
using VoiceTexterBot.Extension;
using VoiceTexterBot.Utilities;

namespace VoiceTexterBot.Services
{
    public class AudioFaleHandler : IFileHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;

        public AudioFaleHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _telegramBotClient = telegramBotClient;
            _appSettings = appSettings;
        }
        public async Task Download(string fileId, CancellationToken ct)
        {
            string inputAudioFilePath = Path.Combine(_appSettings.DownloadsFolder,
                $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");
            using (FileStream destinationStream = File.Create(inputAudioFilePath))
            {
                var file = await _telegramBotClient.GetFileAsync(fileId, ct);
                if (file.FilePath != null)
                    return;
                _telegramBotClient.DownloadFileAsync(file.FilePath, destinationStream, ct);
            }
        }

        public string Process(string languageCode)
        {
            string inputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");
            string outputAudioPath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.OutputAudioFormat}");

            Console.WriteLine("Начинаем конвертацию...");
            AudioConverter.TryConvert(inputAudioPath, outputAudioPath);
            Console.WriteLine("Файл конвертирован");

            Console.WriteLine("Начинаем распознавание...");
            var speechText = SpeechDetector.DetectSpeech(outputAudioPath, _appSettings.InputAudioBitrate, languageCode);
            Console.WriteLine("Файл распознан.");
            return speechText;
        }
    }
}
