using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModifyLaunch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 这是将字符串转换为整数的方法
        private int StringToInt(string input)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                // 转换失败时的处理逻辑，可以是弹出消息框等
                MessageBox.Show("输入无效，请输入一个有效的整数。");
                return 0; // 或者你可以返回其他默认值
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开程序集
                AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly("WowLaunchApp.wtf");

                // 获取命名空间和类名
                string namespaceName = "WowLaunchApp";
                string className = "Utils";

                // 获取要修改的方法名
                string methodName = "InitUtils";

                // 查找目标类
                TypeDefinition targetClass = null;
                foreach (ModuleDefinition module in assembly.Modules)
                {
                    foreach (TypeDefinition type in module.Types)
                    {
                        if (type.Namespace == namespaceName && type.Name == className)
                        {
                            targetClass = type;
                            break;
                        }
                    }
                }

                // 找到目标类后进行操作
                if (targetClass != null)
                {
                    // 查找目标方法
                    MethodDefinition targetMethod = targetClass.Methods.FirstOrDefault(m => m.Name == methodName);
                    if (targetMethod != null && targetMethod.HasBody)
                    {
                        // 获取方法体
                        Mono.Cecil.Cil.MethodBody methodBody = targetMethod.Body;

                        // 获取要修改的指令位置 // debug下标和releas不同, 需要调试
                        Instruction host = methodBody.Instructions[0];
                        Instruction socketPort = methodBody.Instructions[2];
                        Instruction httpPort = methodBody.Instructions[4];
                        int _httpPort = StringToInt(hPort.Text);
                        int _SocketpPort = StringToInt(sPort.Text);
                        // 创建新的指令
                        Instruction new_host = Instruction.Create(OpCodes.Ldstr, hostip.Text);
                        Instruction new_socketPort = Instruction.Create(OpCodes.Ldc_I4, _SocketpPort);
                        Instruction new_httpPort = Instruction.Create(OpCodes.Ldc_I4, _httpPort);

                        // 替换指令
                        methodBody.GetILProcessor().Replace(host, new_host);
                        methodBody.GetILProcessor().Replace(socketPort, new_socketPort);
                        methodBody.GetILProcessor().Replace(httpPort, new_httpPort);
                    }
                }

                // 保存修改后的程序集到新的文件
                assembly.Write("登录器.exe");
                MessageBox.Show("修改完成");
                Application.Exit();
            }
            catch (Exception)
            {
                MessageBox.Show("修改失败...");
            }
            
        }
    }
}
