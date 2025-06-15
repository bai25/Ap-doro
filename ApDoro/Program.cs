using System;
using System.IO;
using Microsoft.Win32;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("正在准备修改登录界面背景...");
            
            // 1. 修改注册表启用自定义登录背景
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background", 
                true))
            {
                if (key == null)
                {
                    using (var newKey = Registry.LocalMachine.CreateSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background"))
                    {
                        newKey.SetValue("OEMBackground", 1, RegistryValueKind.DWord);
                    }
                }
                else
                {
                    key.SetValue("OEMBackground", 1, RegistryValueKind.DWord);
                }
            }

            // 2. 准备目标路径
            string system32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string targetPath = Path.Combine(system32Path, "oobe\\info\\backgrounds");
            
            // 3. 创建目录(如果不存在)
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            // 4. 复制图片到目标位置
            string sourceImagePath = "background.jpg"; 
            string targetImagePath = Path.Combine(targetPath, "backgroundDefault.jpg");
            
            if (!File.Exists(sourceImagePath))
            {
                Console.WriteLine("错误: 未找到背景图片文件 'background.jpg'");
                Console.WriteLine("请将背景图片放在程序同目录下并命名为 background.jpg");
                return;
            }
            
            File.Copy(sourceImagePath, targetImagePath, true);

            Console.WriteLine("操作成功完成!");
            Console.WriteLine("Windows登录界面背景已成功替换");
            Console.WriteLine("请重启电脑或注销账户查看效果");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("错误: 权限不足，请以管理员身份运行此程序");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生错误: {ex.Message}");
            Console.WriteLine("详细信息请查看异常堆栈跟踪");
            Console.WriteLine(ex.StackTrace);
        }
        
        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}
