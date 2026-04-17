Imports System
Imports UnityEngine

' Token: 0x020007BF RID: 1983
Public Class SaltbakerLevelBGSaltHands
	Inherits MonoBehaviour

	' Token: 0x06002CE2 RID: 11490 RVA: 0x001A6ECC File Offset: 0x001A52CC
	Public Sub Play()
		MyBase.transform.position = Me.positions(Me.positionCounter)
		MyBase.transform.localScale = New Vector3(CSng(If((Me.positionCounter <> 0), (-1), 1)), If((Me.positionCounter <> 0), 1.02F, 1F))
		For i As Integer = 0 To Me.rends.Length - 1
			Me.rends(i).sortingOrder = If((Me.positionCounter <> 0), 650, 850) + i * 5
		Next
		Me.anim.Play("SaltHands")
		Me.SFX_SALTB_Bouncer_MakeBouncer()
		Me.positionCounter = 1 - Me.positionCounter
	End Sub

	' Token: 0x06002CE3 RID: 11491 RVA: 0x001A6FA4 File Offset: 0x001A53A4
	Private Sub SFX_SALTB_Bouncer_MakeBouncer()
		AudioManager.Play("sfx_dlc_saltbaker_p3_hands_makebouncer")
	End Sub

	' Token: 0x04003556 RID: 13654
	<SerializeField()>
	Private positions As Vector2()

	' Token: 0x04003557 RID: 13655
	<SerializeField()>
	Private anim As Animator

	' Token: 0x04003558 RID: 13656
	<SerializeField()>
	Private rends As SpriteRenderer()

	' Token: 0x04003559 RID: 13657
	Private positionCounter As Integer
End Class
