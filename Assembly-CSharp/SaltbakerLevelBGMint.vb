Imports System
Imports UnityEngine

' Token: 0x020007BE RID: 1982
Public Class SaltbakerLevelBGMint
	Inherits MonoBehaviour

	' Token: 0x06002CDF RID: 11487 RVA: 0x001A6E30 File Offset: 0x001A5230
	Public Sub StartAnimation(which As Integer)
		Me.anim.Play(which.ToString(), 0, Global.UnityEngine.Random.Range(0F, 0.5F))
	End Sub

	' Token: 0x06002CE0 RID: 11488 RVA: 0x001A6E5C File Offset: 0x001A525C
	Private Sub AniEvent_JumpDown()
		MyBase.transform.position += Me.nextPos.position - Me.startPos.position
		If MyBase.transform.position.y < -1000F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x04003553 RID: 13651
	<SerializeField()>
	Private startPos As Transform

	' Token: 0x04003554 RID: 13652
	<SerializeField()>
	Private nextPos As Transform

	' Token: 0x04003555 RID: 13653
	<SerializeField()>
	Private anim As Animator
End Class
