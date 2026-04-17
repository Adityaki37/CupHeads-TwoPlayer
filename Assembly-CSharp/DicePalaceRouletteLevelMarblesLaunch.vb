Imports System
Imports UnityEngine

' Token: 0x020005E3 RID: 1507
Public Class DicePalaceRouletteLevelMarblesLaunch
	Inherits AbstractMonoBehaviour

	' Token: 0x17000371 RID: 881
	' (set) Token: 0x06001DDE RID: 7646 RVA: 0x00112D2A File Offset: 0x0011112A
	Public WriteOnly Property IsFirstTime As Boolean
		Set(value As Boolean)
			MyBase.animator.SetBool("IsFirstTime", value)
		End Set
	End Property

	' Token: 0x06001DDF RID: 7647 RVA: 0x00112D3D File Offset: 0x0011113D
	Public Sub KillMarblesLaunch()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040026AD RID: 9901
	Private Const IsFirstTimeParameterName As String = "IsFirstTime"
End Class
