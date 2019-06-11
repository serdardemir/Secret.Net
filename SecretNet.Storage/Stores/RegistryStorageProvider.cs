using Microsoft.Win32;
using System;

namespace SecretNet.Storage.Stores
{
    public class RegistryStorageProvider : StorageProvider
    {
        private RegistryHive hive = RegistryHive.CurrentUser;
        private string section;

        public RegistryHive Hive
        {
            get { return hive; }
            set { hive = value; }
        }

        public string Section
        {
            get { return section; }
            set { section = value; }
        }

        public RegistryStorageProvider()
        {
        }

        #region StorageProviderBase Members

        protected override void InternalCreate(string key, object data)
        {
            using (RegistryKey registry = CreateKey())
            {
                registry.SetValue(key, data, RegistryValueKind.Binary);
            }
        }

        protected override void InternalUpdate(string key, object data)
        {
            using (RegistryKey registry = CreateKey())
            {
                registry.SetValue(key, data, RegistryValueKind.Binary);
            }
        }

        protected override void InternalDelete(string key)
        {
            using (RegistryKey registry = CreateKey())
            {
                registry.DeleteValue(key);
            }
        }

        protected override object InternalGet(string key)
        {
            using (RegistryKey registry = OpenKey())
            {
                if (registry == null)
                    return null;
                else
                    return registry.GetValue(key);
            }
        }

        #endregion StorageProviderBase Members

        private RegistryKey GetRegistryRoot(RegistryHive targetHive)
        {
            RegistryKey root = null;

            switch (targetHive)
            {
                case RegistryHive.CurrentUser:
                    root = Microsoft.Win32.Registry.CurrentUser;

                    break;

                case RegistryHive.LocalMachine:
                    root = Microsoft.Win32.Registry.LocalMachine;

                    break;

                case RegistryHive.Users:
                    root = Microsoft.Win32.Registry.Users;

                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        "targetHive",
                        string.Format(
                            "The registry hive {0} is not supported.",
                            hive.ToString()));
            }

            return root;
        }

        private RegistryKey CreateKey()
        {
            RegistryKey root = GetRegistryRoot(hive);
            return root.CreateSubKey(section);
        }

        private RegistryKey OpenKey()
        {
            RegistryKey root = GetRegistryRoot(hive);
            return root.OpenSubKey(section, false);
        }
    }
}