Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000443 RID: 1091
Public NotInheritable Class ObjectPool
	Inherits MonoBehaviour

	' Token: 0x06001006 RID: 4102 RVA: 0x0009E6F0 File Offset: 0x0009CAF0
	Private Sub Awake()
		ObjectPool._instance = Me
		If Me.startupPoolMode = ObjectPool.StartupPoolMode.Awake Then
			ObjectPool.CreateStartupPools()
		End If
	End Sub

	' Token: 0x06001007 RID: 4103 RVA: 0x0009E708 File Offset: 0x0009CB08
	Private Sub Start()
		If Me.startupPoolMode = ObjectPool.StartupPoolMode.Start Then
			ObjectPool.CreateStartupPools()
		End If
	End Sub

	' Token: 0x06001008 RID: 4104 RVA: 0x0009E71C File Offset: 0x0009CB1C
	Private Sub OnDestroy()
		For Each keyValuePair As KeyValuePair(Of GameObject, List(Of GameObject)) In Me.pooledObjects
			ObjectPool.DestroyAll(keyValuePair.Key)
		Next
		Me.spawnedObjects.Clear()
		Me.pooledObjects.Clear()
	End Sub

	' Token: 0x06001009 RID: 4105 RVA: 0x0009E794 File Offset: 0x0009CB94
	Public Shared Sub CreateStartupPools()
		If Not ObjectPool.instance.startupPoolsCreated Then
			ObjectPool.instance.startupPoolsCreated = True
			Dim array As ObjectPool.StartupPool() = ObjectPool.instance.startupPools
			If array IsNot Nothing AndAlso array.Length > 0 Then
				For i As Integer = 0 To array.Length - 1
					ObjectPool.CreatePool(array(i).prefab, array(i).size)
				Next
			End If
		End If
	End Sub

	' Token: 0x0600100A RID: 4106 RVA: 0x0009E7FE File Offset: 0x0009CBFE
	Public Shared Sub CreatePool(Of T As Component)(prefab As T, initialPoolSize As Integer)
		ObjectPool.CreatePool(prefab.gameObject, initialPoolSize)
	End Sub

	' Token: 0x0600100B RID: 4107 RVA: 0x0009E814 File Offset: 0x0009CC14
	Public Shared Sub CreatePool(prefab As GameObject, initialPoolSize As Integer)
		If prefab IsNot Nothing AndAlso Not ObjectPool.instance.pooledObjects.ContainsKey(prefab) Then
			Dim list As List(Of GameObject) = New List(Of GameObject)()
			ObjectPool.instance.pooledObjects.Add(prefab, list)
			If initialPoolSize > 0 Then
				Dim activeSelf As Boolean = prefab.activeSelf
				prefab.SetActive(True)
				Dim transform As Transform = ObjectPool.instance.transform
				While list.Count < initialPoolSize
					Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(prefab)
					gameObject.SetActive(False)
					gameObject.transform.parent = transform
					list.Add(gameObject)
				End While
				prefab.SetActive(activeSelf)
			End If
		End If
	End Sub

	' Token: 0x0600100C RID: 4108 RVA: 0x0009E8B2 File Offset: 0x0009CCB2
	Public Shared Function Spawn(Of T As Component)(prefab As T, parent As Transform, position As Vector3, rotation As Quaternion) As T
		Return ObjectPool.Spawn(prefab.gameObject, parent, position, rotation).GetComponent(Of T)()
	End Function

	' Token: 0x0600100D RID: 4109 RVA: 0x0009E8CE File Offset: 0x0009CCCE
	Public Shared Function Spawn(Of T As Component)(prefab As T, position As Vector3, rotation As Quaternion) As T
		Return ObjectPool.Spawn(prefab.gameObject, Nothing, position, rotation).GetComponent(Of T)()
	End Function

	' Token: 0x0600100E RID: 4110 RVA: 0x0009E8EA File Offset: 0x0009CCEA
	Public Shared Function Spawn(Of T As Component)(prefab As T, parent As Transform, position As Vector3) As T
		Return ObjectPool.Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent(Of T)()
	End Function

	' Token: 0x0600100F RID: 4111 RVA: 0x0009E90A File Offset: 0x0009CD0A
	Public Shared Function Spawn(Of T As Component)(prefab As T, position As Vector3) As T
		Return ObjectPool.Spawn(prefab.gameObject, Nothing, position, Quaternion.identity).GetComponent(Of T)()
	End Function

	' Token: 0x06001010 RID: 4112 RVA: 0x0009E92A File Offset: 0x0009CD2A
	Public Shared Function Spawn(Of T As Component)(prefab As T, parent As Transform) As T
		Return ObjectPool.Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent(Of T)()
	End Function

	' Token: 0x06001011 RID: 4113 RVA: 0x0009E94E File Offset: 0x0009CD4E
	Public Shared Function Spawn(Of T As Component)(prefab As T) As T
		Return ObjectPool.Spawn(prefab.gameObject, Nothing, Vector3.zero, Quaternion.identity).GetComponent(Of T)()
	End Function

	' Token: 0x06001012 RID: 4114 RVA: 0x0009E974 File Offset: 0x0009CD74
	Public Shared Function Spawn(prefab As GameObject, parent As Transform, position As Vector3, rotation As Quaternion) As GameObject
		Dim list As List(Of GameObject)
		Dim gameObject As GameObject
		Dim transform As Transform
		If ObjectPool.instance.pooledObjects.TryGetValue(prefab, list) Then
			gameObject = Nothing
			If list.Count > 0 Then
				While gameObject Is Nothing AndAlso list.Count > 0
					gameObject = list(0)
					list.RemoveAt(0)
				End While
				If gameObject IsNot Nothing Then
					transform = gameObject.transform
					transform.parent = parent
					transform.localPosition = position
					transform.localRotation = rotation
					gameObject.SetActive(True)
					ObjectPool.instance.spawnedObjects.Add(gameObject, prefab)
					Return gameObject
				End If
			End If
			gameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(prefab)
			transform = gameObject.transform
			transform.parent = parent
			transform.localPosition = position
			transform.localRotation = rotation
			ObjectPool.instance.spawnedObjects.Add(gameObject, prefab)
			Return gameObject
		End If
		gameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(prefab)
		transform = gameObject.GetComponent(Of Transform)()
		transform.parent = parent
		transform.localPosition = position
		transform.localRotation = rotation
		Return gameObject
	End Function

	' Token: 0x06001013 RID: 4115 RVA: 0x0009EA6E File Offset: 0x0009CE6E
	Public Shared Function Spawn(prefab As GameObject, parent As Transform, position As Vector3) As GameObject
		Return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity)
	End Function

	' Token: 0x06001014 RID: 4116 RVA: 0x0009EA7D File Offset: 0x0009CE7D
	Public Shared Function Spawn(prefab As GameObject, position As Vector3, rotation As Quaternion) As GameObject
		Return ObjectPool.Spawn(prefab, Nothing, position, rotation)
	End Function

	' Token: 0x06001015 RID: 4117 RVA: 0x0009EA88 File Offset: 0x0009CE88
	Public Shared Function Spawn(prefab As GameObject, parent As Transform) As GameObject
		Return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity)
	End Function

	' Token: 0x06001016 RID: 4118 RVA: 0x0009EA9B File Offset: 0x0009CE9B
	Public Shared Function Spawn(prefab As GameObject, position As Vector3) As GameObject
		Return ObjectPool.Spawn(prefab, Nothing, position, Quaternion.identity)
	End Function

	' Token: 0x06001017 RID: 4119 RVA: 0x0009EAAA File Offset: 0x0009CEAA
	Public Shared Function Spawn(prefab As GameObject) As GameObject
		Return ObjectPool.Spawn(prefab, Nothing, Vector3.zero, Quaternion.identity)
	End Function

	' Token: 0x06001018 RID: 4120 RVA: 0x0009EABD File Offset: 0x0009CEBD
	Public Shared Sub Recycle(Of T As Component)(obj As T)
		ObjectPool.Recycle(obj.gameObject)
	End Sub

	' Token: 0x06001019 RID: 4121 RVA: 0x0009EAD4 File Offset: 0x0009CED4
	Public Shared Sub Recycle(obj As GameObject)
		Dim gameObject As GameObject
		If ObjectPool.instance.spawnedObjects.TryGetValue(obj, gameObject) Then
			ObjectPool.Recycle(obj, gameObject)
		Else
			Global.UnityEngine.[Object].Destroy(obj)
		End If
	End Sub

	' Token: 0x0600101A RID: 4122 RVA: 0x0009EB0C File Offset: 0x0009CF0C
	Private Shared Sub Recycle(obj As GameObject, prefab As GameObject)
		ObjectPool.instance.pooledObjects(prefab).Add(obj)
		ObjectPool.instance.spawnedObjects.Remove(obj)
		If obj IsNot Nothing Then
			obj.transform.parent = ObjectPool.instance.transform
			obj.SetActive(False)
		End If
	End Sub

	' Token: 0x0600101B RID: 4123 RVA: 0x0009EB68 File Offset: 0x0009CF68
	Public Shared Sub RecycleAll(Of T As Component)(prefab As T)
		ObjectPool.RecycleAll(prefab.gameObject)
	End Sub

	' Token: 0x0600101C RID: 4124 RVA: 0x0009EB7C File Offset: 0x0009CF7C
	Public Shared Sub RecycleAll(prefab As GameObject)
		For Each keyValuePair As KeyValuePair(Of GameObject, GameObject) In ObjectPool.instance.spawnedObjects
			If keyValuePair.Value Is prefab Then
				ObjectPool.tempList.Add(keyValuePair.Key)
			End If
		Next
		For i As Integer = 0 To ObjectPool.tempList.Count - 1
			ObjectPool.Recycle(ObjectPool.tempList(i))
		Next
		ObjectPool.tempList.Clear()
	End Sub

	' Token: 0x0600101D RID: 4125 RVA: 0x0009EC30 File Offset: 0x0009D030
	Public Shared Sub RecycleAll()
		ObjectPool.tempList.AddRange(ObjectPool.instance.spawnedObjects.Keys)
		For i As Integer = 0 To ObjectPool.tempList.Count - 1
			ObjectPool.Recycle(ObjectPool.tempList(i))
		Next
		ObjectPool.tempList.Clear()
	End Sub

	' Token: 0x0600101E RID: 4126 RVA: 0x0009EC8B File Offset: 0x0009D08B
	Public Shared Function IsSpawned(obj As GameObject) As Boolean
		Return ObjectPool.instance.spawnedObjects.ContainsKey(obj)
	End Function

	' Token: 0x0600101F RID: 4127 RVA: 0x0009EC9D File Offset: 0x0009D09D
	Public Shared Function CountPooled(Of T As Component)(prefab As T) As Integer
		Return ObjectPool.CountPooled(prefab.gameObject)
	End Function

	' Token: 0x06001020 RID: 4128 RVA: 0x0009ECB4 File Offset: 0x0009D0B4
	Public Shared Function CountPooled(prefab As GameObject) As Integer
		Dim list As List(Of GameObject)
		If ObjectPool.instance.pooledObjects.TryGetValue(prefab, list) Then
			Return list.Count
		End If
		Return 0
	End Function

	' Token: 0x06001021 RID: 4129 RVA: 0x0009ECE0 File Offset: 0x0009D0E0
	Public Shared Function CountSpawned(Of T As Component)(prefab As T) As Integer
		Return ObjectPool.CountSpawned(prefab.gameObject)
	End Function

	' Token: 0x06001022 RID: 4130 RVA: 0x0009ECF4 File Offset: 0x0009D0F4
	Public Shared Function CountSpawned(prefab As GameObject) As Integer
		Dim num As Integer = 0
		For Each gameObject As GameObject In ObjectPool.instance.spawnedObjects.Values
			If prefab Is gameObject Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06001023 RID: 4131 RVA: 0x0009ED68 File Offset: 0x0009D168
	Public Shared Function CountAllPooled() As Integer
		Dim num As Integer = 0
		For Each list As List(Of GameObject) In ObjectPool.instance.pooledObjects.Values
			num += list.Count
		Next
		Return num
	End Function

	' Token: 0x06001024 RID: 4132 RVA: 0x0009EDD4 File Offset: 0x0009D1D4
	Public Shared Function GetPooled(prefab As GameObject, list As List(Of GameObject), appendList As Boolean) As List(Of GameObject)
		If list Is Nothing Then
			list = New List(Of GameObject)()
		End If
		If Not appendList Then
			list.Clear()
		End If
		Dim list2 As List(Of GameObject)
		If ObjectPool.instance.pooledObjects.TryGetValue(prefab, list2) Then
			list.AddRange(list2)
		End If
		Return list
	End Function

	' Token: 0x06001025 RID: 4133 RVA: 0x0009EE1C File Offset: 0x0009D21C
	Public Shared Function GetPooled(Of T As Component)(prefab As T, list As List(Of T), appendList As Boolean) As List(Of T)
		If list Is Nothing Then
			list = New List(Of T)()
		End If
		If Not appendList Then
			list.Clear()
		End If
		Dim list2 As List(Of GameObject)
		If ObjectPool.instance.pooledObjects.TryGetValue(prefab.gameObject, list2) Then
			For i As Integer = 0 To list2.Count - 1
				list.Add(list2(i).GetComponent(Of T)())
			Next
		End If
		Return list
	End Function

	' Token: 0x06001026 RID: 4134 RVA: 0x0009EE90 File Offset: 0x0009D290
	Public Shared Function GetSpawned(prefab As GameObject, list As List(Of GameObject), appendList As Boolean) As List(Of GameObject)
		If list Is Nothing Then
			list = New List(Of GameObject)()
		End If
		If Not appendList Then
			list.Clear()
		End If
		For Each keyValuePair As KeyValuePair(Of GameObject, GameObject) In ObjectPool.instance.spawnedObjects
			If keyValuePair.Value Is prefab Then
				list.Add(keyValuePair.Key)
			End If
		Next
		Return list
	End Function

	' Token: 0x06001027 RID: 4135 RVA: 0x0009EF24 File Offset: 0x0009D324
	Public Shared Function GetSpawned(Of T As Component)(prefab As T, list As List(Of T), appendList As Boolean) As List(Of T)
		If list Is Nothing Then
			list = New List(Of T)()
		End If
		If Not appendList Then
			list.Clear()
		End If
		Dim gameObject As GameObject = prefab.gameObject
		For Each keyValuePair As KeyValuePair(Of GameObject, GameObject) In ObjectPool.instance.spawnedObjects
			If keyValuePair.Value Is gameObject Then
				list.Add(keyValuePair.Key.GetComponent(Of T)())
			End If
		Next
		Return list
	End Function

	' Token: 0x06001028 RID: 4136 RVA: 0x0009EFCC File Offset: 0x0009D3CC
	Public Shared Sub DestroyPooled(prefab As GameObject)
		Dim list As List(Of GameObject)
		If ObjectPool.instance.pooledObjects.TryGetValue(prefab, list) Then
			For i As Integer = 0 To list.Count - 1
				Global.UnityEngine.[Object].Destroy(list(i))
			Next
			list.Clear()
		End If
	End Sub

	' Token: 0x06001029 RID: 4137 RVA: 0x0009F019 File Offset: 0x0009D419
	Public Shared Sub DestroyPooled(Of T As Component)(prefab As T)
		ObjectPool.DestroyPooled(prefab.gameObject)
	End Sub

	' Token: 0x0600102A RID: 4138 RVA: 0x0009F02D File Offset: 0x0009D42D
	Public Shared Sub DestroyAll(prefab As GameObject)
		ObjectPool.RecycleAll(prefab)
		ObjectPool.DestroyPooled(prefab)
	End Sub

	' Token: 0x0600102B RID: 4139 RVA: 0x0009F03B File Offset: 0x0009D43B
	Public Shared Sub DestroyAll(Of T As Component)(prefab As T)
		ObjectPool.DestroyAll(prefab.gameObject)
	End Sub

	' Token: 0x1700028F RID: 655
	' (get) Token: 0x0600102C RID: 4140 RVA: 0x0009F050 File Offset: 0x0009D450
	Public Shared ReadOnly Property instance As ObjectPool
		Get
			If ObjectPool._instance IsNot Nothing Then
				Return ObjectPool._instance
			End If
			ObjectPool._instance = Global.UnityEngine.[Object].FindObjectOfType(Of ObjectPool)()
			If ObjectPool._instance IsNot Nothing Then
				Return ObjectPool._instance
			End If
			ObjectPool._instance = New GameObject("ObjectPool") With { .transform = { .localPosition = Vector3.zero, .localRotation = Quaternion.identity, .localScale = Vector3.one } }.AddComponent(Of ObjectPool)()
			Return ObjectPool._instance
		End Get
	End Property

	' Token: 0x0400199E RID: 6558
	Private Shared _instance As ObjectPool

	' Token: 0x0400199F RID: 6559
	Private Shared tempList As List(Of GameObject) = New List(Of GameObject)()

	' Token: 0x040019A0 RID: 6560
	Private pooledObjects As Dictionary(Of GameObject, List(Of GameObject)) = New Dictionary(Of GameObject, List(Of GameObject))()

	' Token: 0x040019A1 RID: 6561
	Private spawnedObjects As Dictionary(Of GameObject, GameObject) = New Dictionary(Of GameObject, GameObject)()

	' Token: 0x040019A2 RID: 6562
	Public startupPoolMode As ObjectPool.StartupPoolMode

	' Token: 0x040019A3 RID: 6563
	Public startupPools As ObjectPool.StartupPool()

	' Token: 0x040019A4 RID: 6564
	Private startupPoolsCreated As Boolean

	' Token: 0x02000444 RID: 1092
	Public Enum StartupPoolMode
		' Token: 0x040019A6 RID: 6566
		Awake
		' Token: 0x040019A7 RID: 6567
		Start
		' Token: 0x040019A8 RID: 6568
		CallManually
	End Enum

	' Token: 0x02000445 RID: 1093
	<Serializable()>
	Public Class StartupPool
		' Token: 0x040019A9 RID: 6569
		Public size As Integer

		' Token: 0x040019AA RID: 6570
		Public prefab As GameObject
	End Class
End Class
