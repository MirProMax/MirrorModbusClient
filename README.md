# 关于这个仓库
- 这是一个适用于Unity3D的Package
- 可以利用此Package从Modbus TCP Server中读取数据或写入数据。

# 如何使用？
- 打开Unity Package Manager
- 点击左上角的“+”图标
- 选择“Git URL”
- 在你的CSharp文件开头加入“using MirrorModbus”
- 在你的CSharp文件中实例化一个MirrorModbusClient对象，并使用该对象包含的方法，对Modbus TCP Server进行读写操作。

# 关于关键的文件
- EasyModbus.dll是基础文件。
- MirrorModbusClient.cs额外提供了方便调用的方法。
- 上述两个文件可以直接导入Unity Project的Asset文件夹中使用。

# 如果你也想开发一个用于Unity3D的自定义Package
- 添加MirrorModbus.asmdef和package.json即可被Unity Package Manager识别
