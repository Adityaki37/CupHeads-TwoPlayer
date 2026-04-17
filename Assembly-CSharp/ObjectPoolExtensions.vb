Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000446 RID: 1094
Public Module ObjectPoolExtensions
	' Token: 0x0600102F RID: 4143 RVA: 0x0009F0F2 File Offset: 0x0009D4F2
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub CreatePool(Of T As Component)(prefab As T)
		ObjectPool.CreatePool(Of T)(prefab, 0)
	End Sub

	' Token: 0x06001030 RID: 4144 RVA: 0x0009F0FB File Offset: 0x0009D4FB
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub CreatePool(Of T As Component)(prefab As T, initialPoolSize As Integer)
		ObjectPool.CreatePool(Of T)(prefab, initialPoolSize)
	End Sub

	' Token: 0x06001031 RID: 4145 RVA: 0x0009F104 File Offset: 0x0009D504
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub CreatePool(prefab As GameObject)
		ObjectPool.CreatePool(prefab, 0)
	End Sub

	' Token: 0x06001032 RID: 4146 RVA: 0x0009F10D File Offset: 0x0009D50D
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub CreatePool(prefab As GameObject, initialPoolSize As Integer)
		ObjectPool.CreatePool(prefab, initialPoolSize)
	End Sub

	' Token: 0x06001033 RID: 4147 RVA: 0x0009F116 File Offset: 0x0009D516
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(Of T As Component)(prefab As T, parent As Transform, position As Vector3, rotation As Quaternion) As T
		Return ObjectPool.Spawn(Of T)(prefab, parent, position, rotation)
	End Function

	' Token: 0x06001034 RID: 4148 RVA: 0x0009F121 File Offset: 0x0009D521
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(Of T As Component)(prefab As T, position As Vector3, rotation As Quaternion) As T
		Return ObjectPool.Spawn(Of T)(prefab, Nothing, position, rotation)
	End Function

	' Token: 0x06001035 RID: 4149 RVA: 0x0009F12C File Offset: 0x0009D52C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(Of T As Component)(prefab As T, parent As Transform, position As Vector3) As T
		Return ObjectPool.Spawn(Of T)(prefab, parent, position, Quaternion.identity)
	End Function

	' Token: 0x06001036 RID: 4150 RVA: 0x0009F13B File Offset: 0x0009D53B
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(Of T As Component)(prefab As T, position As Vector3) As T
		Return ObjectPool.Spawn(Of T)(prefab, Nothing, position, Quaternion.identity)
	End Function

	' Token: 0x06001037 RID: 4151 RVA: 0x0009F14A File Offset: 0x0009D54A
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(Of T As Component)(prefab As T, parent As Transform) As T
		Return ObjectPool.Spawn(Of T)(prefab, parent, Vector3.zero, Quaternion.identity)
	End Function

	' Token: 0x06001038 RID: 4152 RVA: 0x0009F15D File Offset: 0x0009D55D
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(Of T As Component)(prefab As T) As T
		Return ObjectPool.Spawn(Of T)(prefab, Nothing, Vector3.zero, Quaternion.identity)
	End Function

	' Token: 0x06001039 RID: 4153 RVA: 0x0009F170 File Offset: 0x0009D570
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(prefab As GameObject, parent As Transform, position As Vector3, rotation As Quaternion) As GameObject
		Return ObjectPool.Spawn(prefab, parent, position, rotation)
	End Function

	' Token: 0x0600103A RID: 4154 RVA: 0x0009F17B File Offset: 0x0009D57B
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(prefab As GameObject, position As Vector3, rotation As Quaternion) As GameObject
		Return ObjectPool.Spawn(prefab, Nothing, position, rotation)
	End Function

	' Token: 0x0600103B RID: 4155 RVA: 0x0009F186 File Offset: 0x0009D586
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(prefab As GameObject, parent As Transform, position As Vector3) As GameObject
		Return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity)
	End Function

	' Token: 0x0600103C RID: 4156 RVA: 0x0009F195 File Offset: 0x0009D595
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(prefab As GameObject, position As Vector3) As GameObject
		Return ObjectPool.Spawn(prefab, Nothing, position, Quaternion.identity)
	End Function

	' Token: 0x0600103D RID: 4157 RVA: 0x0009F1A4 File Offset: 0x0009D5A4
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(prefab As GameObject, parent As Transform) As GameObject
		Return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity)
	End Function

	' Token: 0x0600103E RID: 4158 RVA: 0x0009F1B7 File Offset: 0x0009D5B7
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Spawn(prefab As GameObject) As GameObject
		Return ObjectPool.Spawn(prefab, Nothing, Vector3.zero, Quaternion.identity)
	End Function

	' Token: 0x0600103F RID: 4159 RVA: 0x0009F1CA File Offset: 0x0009D5CA
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub Recycle(Of T As Component)(obj As T)
		ObjectPool.Recycle(Of T)(obj)
	End Sub

	' Token: 0x06001040 RID: 4160 RVA: 0x0009F1D2 File Offset: 0x0009D5D2
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub Recycle(obj As GameObject)
		ObjectPool.Recycle(obj)
	End Sub

	' Token: 0x06001041 RID: 4161 RVA: 0x0009F1DA File Offset: 0x0009D5DA
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub RecycleAll(Of T As Component)(prefab As T)
		ObjectPool.RecycleAll(Of T)(prefab)
	End Sub

	' Token: 0x06001042 RID: 4162 RVA: 0x0009F1E2 File Offset: 0x0009D5E2
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub RecycleAll(prefab As GameObject)
		ObjectPool.RecycleAll(prefab)
	End Sub

	' Token: 0x06001043 RID: 4163 RVA: 0x0009F1EA File Offset: 0x0009D5EA
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function CountPooled(Of T As Component)(prefab As T) As Integer
		Return ObjectPool.CountPooled(Of T)(prefab)
	End Function

	' Token: 0x06001044 RID: 4164 RVA: 0x0009F1F2 File Offset: 0x0009D5F2
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function CountPooled(prefab As GameObject) As Integer
		Return ObjectPool.CountPooled(prefab)
	End Function

	' Token: 0x06001045 RID: 4165 RVA: 0x0009F1FA File Offset: 0x0009D5FA
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function CountSpawned(Of T As Component)(prefab As T) As Integer
		Return ObjectPool.CountSpawned(Of T)(prefab)
	End Function

	' Token: 0x06001046 RID: 4166 RVA: 0x0009F202 File Offset: 0x0009D602
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function CountSpawned(prefab As GameObject) As Integer
		Return ObjectPool.CountSpawned(prefab)
	End Function

	' Token: 0x06001047 RID: 4167 RVA: 0x0009F20A File Offset: 0x0009D60A
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetSpawned(prefab As GameObject, list As List(Of GameObject), appendList As Boolean) As List(Of GameObject)
		Return ObjectPool.GetSpawned(prefab, list, appendList)
	End Function

	' Token: 0x06001048 RID: 4168 RVA: 0x0009F214 File Offset: 0x0009D614
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetSpawned(prefab As GameObject, list As List(Of GameObject)) As List(Of GameObject)
		Return ObjectPool.GetSpawned(prefab, list, False)
	End Function

	' Token: 0x06001049 RID: 4169 RVA: 0x0009F21E File Offset: 0x0009D61E
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetSpawned(prefab As GameObject) As List(Of GameObject)
		Return ObjectPool.GetSpawned(prefab, Nothing, False)
	End Function

	' Token: 0x0600104A RID: 4170 RVA: 0x0009F228 File Offset: 0x0009D628
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetSpawned(Of T As Component)(prefab As T, list As List(Of T), appendList As Boolean) As List(Of T)
		Return ObjectPool.GetSpawned(Of T)(prefab, list, appendList)
	End Function

	' Token: 0x0600104B RID: 4171 RVA: 0x0009F232 File Offset: 0x0009D632
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetSpawned(Of T As Component)(prefab As T, list As List(Of T)) As List(Of T)
		Return ObjectPool.GetSpawned(Of T)(prefab, list, False)
	End Function

	' Token: 0x0600104C RID: 4172 RVA: 0x0009F23C File Offset: 0x0009D63C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetSpawned(Of T As Component)(prefab As T) As List(Of T)
		Return ObjectPool.GetSpawned(Of T)(prefab, Nothing, False)
	End Function

	' Token: 0x0600104D RID: 4173 RVA: 0x0009F246 File Offset: 0x0009D646
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetPooled(prefab As GameObject, list As List(Of GameObject), appendList As Boolean) As List(Of GameObject)
		Return ObjectPool.GetPooled(prefab, list, appendList)
	End Function

	' Token: 0x0600104E RID: 4174 RVA: 0x0009F250 File Offset: 0x0009D650
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetPooled(prefab As GameObject, list As List(Of GameObject)) As List(Of GameObject)
		Return ObjectPool.GetPooled(prefab, list, False)
	End Function

	' Token: 0x0600104F RID: 4175 RVA: 0x0009F25A File Offset: 0x0009D65A
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetPooled(prefab As GameObject) As List(Of GameObject)
		Return ObjectPool.GetPooled(prefab, Nothing, False)
	End Function

	' Token: 0x06001050 RID: 4176 RVA: 0x0009F264 File Offset: 0x0009D664
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetPooled(Of T As Component)(prefab As T, list As List(Of T), appendList As Boolean) As List(Of T)
		Return ObjectPool.GetPooled(Of T)(prefab, list, appendList)
	End Function

	' Token: 0x06001051 RID: 4177 RVA: 0x0009F26E File Offset: 0x0009D66E
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetPooled(Of T As Component)(prefab As T, list As List(Of T)) As List(Of T)
		Return ObjectPool.GetPooled(Of T)(prefab, list, False)
	End Function

	' Token: 0x06001052 RID: 4178 RVA: 0x0009F278 File Offset: 0x0009D678
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetPooled(Of T As Component)(prefab As T) As List(Of T)
		Return ObjectPool.GetPooled(Of T)(prefab, Nothing, False)
	End Function

	' Token: 0x06001053 RID: 4179 RVA: 0x0009F282 File Offset: 0x0009D682
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub DestroyPooled(prefab As GameObject)
		ObjectPool.DestroyPooled(prefab)
	End Sub

	' Token: 0x06001054 RID: 4180 RVA: 0x0009F28A File Offset: 0x0009D68A
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub DestroyPooled(Of T As Component)(prefab As T)
		ObjectPool.DestroyPooled(prefab.gameObject)
	End Sub

	' Token: 0x06001055 RID: 4181 RVA: 0x0009F29E File Offset: 0x0009D69E
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub DestroyAll(prefab As GameObject)
		ObjectPool.DestroyAll(prefab)
	End Sub

	' Token: 0x06001056 RID: 4182 RVA: 0x0009F2A6 File Offset: 0x0009D6A6
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub DestroyAll(Of T As Component)(prefab As T)
		ObjectPool.DestroyAll(prefab.gameObject)
	End Sub
End Module
