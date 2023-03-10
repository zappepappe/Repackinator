using Microsoft.Win32;
using Repackinator.Helpers;

namespace Repackinator.Shell
{
    public class ContextMenu
    {
        private static void RegisterSubMenu(RegistryKey key, string name, string description, string command)
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }
            var menu1Key = key.CreateSubKey($"shell\\{name}");
            menu1Key.SetValue("MUIVerb", description);
            menu1Key.SetValue("AppliesTo", extension);
            var commandMenu1Key = menu1Key.CreateSubKey("command");
            commandMenu1Key.SetValue(null, command);
        }

        public static bool RegisterContext()
        {
            if (!OperatingSystem.IsWindows() || !Utility.IsAdmin())
            {
                return false;
            }
            var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{AppDomain.CurrentDomain.FriendlyName}.exe");

            using var key = Registry.ClassesRoot.CreateSubKey($"*\\shell\\Repackinator");
            
            key.SetValue("AppliesTo", ".iso OR .cci OR .cso");
            key.SetValue("MUIVerb", "Repackinator");
            key.SetValue("SubCommands", string.Empty);

            RegisterSubMenu(key, "01menu", "Convert To ISO", $"\"{exePath}\" -i=\"%L\" -a=convert -w");
            RegisterSubMenu(key, "02menu", "Convert To ISO (Scrub)", $"\"{exePath}\" -i=\"%L\" -a=convert -s=Scrub -w");
            RegisterSubMenu(key, "03menu", "Convert To ISO (TrimmedScrub)", $"\"{exePath}\" -i=\"%L\" -a=convert -s=TrimmedScrub -w");

            RegisterSubMenu(key, "04menu", "Convert To CCI", $"\"{exePath}\" -i=\"%L\" -a=convert -c -w");
            RegisterSubMenu(key, "05menu", "Convert To CCI (Scrub)", $"\"{exePath}\" -i=\"%L\" -a=convert -s=Scrub -c -w");
            RegisterSubMenu(key, "06menu", "Convert To CCI (TrimmedScrub)", $"\"{exePath}\" -i=\"%L\" -a=convert -s=TrimmedScrub -c -w");

            RegisterSubMenu(key, "07menu", "Compare Set First", $"\"{exePath}\" -f=\"%L\" -a=compare");
            RegisterSubMenu(key, "08menu", "Compare First With", $"\"{exePath}\" -s=\"%L\" -a=compare -c -w");

            RegisterSubMenu(key, "09menu", "Info", $"\"{exePath}\" -i=\"%L\" -a=info -w");

            RegisterSubMenu(key, "10menu", "Checksum Sector Data (SHA256)", $"\"{exePath}\" -i=\"%L\" -a=checksum -w");

            RegisterSubMenu(key, "11menu", "Extract", $"\"{exePath}\" -i=\"%L\" -a=extract -w");

            return true;            
        }

        public static bool UnregisterContext()
        {
            if (!OperatingSystem.IsWindows() || !Utility.IsAdmin())
            {
                return false;
            }
            Registry.ClassesRoot.DeleteSubKeyTree("*\\shell\\Repackinator");
            return true;
        }
    }
}
