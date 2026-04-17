Imports System
Imports UnityEngine

' Token: 0x02000441 RID: 1089
Public Class CupheadEventSystem
	Inherits AbstractMonoBehaviour

	' Token: 0x06001000 RID: 4096 RVA: 0x0009E599 File Offset: 0x0009C999
	Public Shared Sub Init()
		If CupheadEventSystem._instance IsNot Nothing Then
			Return
		End If
		CupheadEventSystem._instance = TryCast(Global.UnityEngine.[Object].Instantiate(Resources.Load("EventSystems/CupheadEventSystem")), GameObject).GetComponent(Of CupheadEventSystem)()
	End Sub

	' Token: 0x06001001 RID: 4097 RVA: 0x0009E5CC File Offset: 0x0009C9CC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If CupheadEventSystem._instance Is Nothing Then
			CupheadEventSystem._instance = Me
			MyBase.gameObject.name = MyBase.gameObject.name.Replace("(Clone)", String.Empty)
			Return
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400199C RID: 6556
	Private Const PATH As String = "EventSystems/CupheadEventSystem"

	' Token: 0x0400199D RID: 6557
	Private Shared _instance As CupheadEventSystem
End Class
