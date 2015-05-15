using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LogoFX.UI.Views.Infra.Localization
{
    internal class WinRes
    {
        #region Win32 Constants

        private const uint RT_CURSOR = 0x00000001;
        private const uint RT_BITMAP = 0x00000002;
        private const uint RT_ICON = 0x00000003;
        private const uint RT_MENU = 0x00000004;
        private const uint RT_DIALOG = 0x00000005;
        private const uint RT_STRING = 0x00000006;
        private const uint RT_FONTDIR = 0x00000007;
        private const uint RT_FONT = 0x00000008;
        private const uint RT_ACCELERATOR = 0x00000009;
        private const uint RT_RCDATA = 0x0000000a;
        private const uint RT_MESSAGETABLE = 0x0000000b;

        private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

        #endregion

        #region Win32 Functions

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int LoadString(IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool EnumResourceNamesWithID(
            IntPtr hModule,
            uint lpszType,
            EnumResNameDelegate lpEnumFunc,
            IntPtr lParam);

        private delegate bool EnumResNameDelegate(
            IntPtr hModule,
            IntPtr lpszType,
            IntPtr lpszName,
            IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern bool EnumResourceLanguages(
            IntPtr hModule,
            IntPtr lpszType,
            IntPtr lpName,
            EnumResLangDelegate lpEnumFunc,
            IntPtr lParam);

        private delegate bool EnumResLangDelegate(
            IntPtr hModule,
            IntPtr lpszType,
            IntPtr lpszName,
            ushort wIdLanguage,
            IntPtr lParam);

        [DllImport("Kernel32.dll", EntryPoint = "FindResourceW", SetLastError = true)]
        private static extern IntPtr FindResource(IntPtr hModule, IntPtr pName, IntPtr pType);

        [DllImport("Kernel32.dll", EntryPoint = "LoadResource", SetLastError = true)]
        private static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResource);

        [DllImport("Kernel32.dll", EntryPoint = "LockResource")]
        private static extern IntPtr LockResource(IntPtr hGlobal);

        [DllImport("Kernel32.dll", EntryPoint = "SizeofResource", SetLastError = true)]
        private static extern uint SizeofResource(IntPtr hModule, IntPtr hResource);

        #endregion

        #region Fields

        private ResourceSetCollection _cache;

        #endregion

        #region Public Methods

        public ResourceSetCollection EnumStringResources(string libraryFileName)
        {
            IntPtr hMod = LoadLibraryEx(libraryFileName, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);

            _cache = new ResourceSetCollection();

            try
            {
                if (!EnumResourceNamesWithID(hMod, RT_STRING, EnumRes, IntPtr.Zero))
                {
                    //throw new Win32Exception();
                    _cache = null;
                }

                return _cache;
            }

            finally
            {
                _cache = null;
                FreeLibrary(hMod);
            }
        }

        #endregion

        #region Private Members

        private bool IS_INTRESOURCE(IntPtr value)
        {
            return ((uint)value) <= ushort.MaxValue;
        }

        private uint GET_RESOURCE_ID(IntPtr value)
        {
            if (IS_INTRESOURCE(value))
            {
                return (uint)value;
            }

            throw new NotSupportedException("Value is not an ID!");
        }

        private bool EnumRes(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam)
        {
            LoadStringTable(lpszName, hModule);
            return true;
        }

        private void LoadStringTable(IntPtr lpszName, IntPtr hModule)
        {
            uint id = GET_RESOURCE_ID(lpszName);
            id = (id - 1) * 16;

            ResourceCollection resourceCollection = new ResourceCollection();
            _cache.Add(id.ToString() + ".resources", resourceCollection);

            for (int i = 0; i < 16; ++i)
            {
                StringBuilder stringBuilder = new StringBuilder(255);
                int retVal = LoadString(hModule, id, stringBuilder, stringBuilder.Capacity + 1);

                if (retVal != 0)
                {
                    resourceCollection[id.ToString()] = stringBuilder.ToString();
                }
                ++id;
            }
        }

        #endregion
    }
}
