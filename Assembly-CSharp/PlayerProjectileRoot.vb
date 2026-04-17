Imports System
Imports UnityEngine

' Token: 0x02000A4D RID: 2637
Public Class PlayerProjectileRoot
	Inherits AbstractMonoBehaviour

	' Token: 0x1700056A RID: 1386
	' (get) Token: 0x06003EC8 RID: 16072 RVA: 0x002266F6 File Offset: 0x00224AF6
	Public ReadOnly Property Position As Vector2
		Get
			Return MyBase.transform.position
		End Get
	End Property

	' Token: 0x1700056B RID: 1387
	' (get) Token: 0x06003EC9 RID: 16073 RVA: 0x00226708 File Offset: 0x00224B08
	Public ReadOnly Property Rotation As Single
		Get
			Return MyBase.transform.eulerAngles.z
		End Get
	End Property

	' Token: 0x1700056C RID: 1388
	' (get) Token: 0x06003ECA RID: 16074 RVA: 0x00226728 File Offset: 0x00224B28
	Public ReadOnly Property Scale As Vector3
		Get
			Dim num As Single = 1F
			Return New Vector3(1F, num, 1F)
		End Get
	End Property
End Class
