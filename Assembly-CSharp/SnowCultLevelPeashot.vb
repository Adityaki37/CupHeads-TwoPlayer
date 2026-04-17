Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007EF RID: 2031
Public Class SnowCultLevelPeashot
	Inherits BasicProjectile

	' Token: 0x06002E9C RID: 11932 RVA: 0x001B7C7F File Offset: 0x001B607F
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.SFX_SNOWCULT_TarotCardTravelLoop()
	End Sub

	' Token: 0x06002E9D RID: 11933 RVA: 0x001B7C90 File Offset: 0x001B6090
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		If parryable Then
			Dim num As Integer = Global.UnityEngine.Random.Range(0, 2)
			If num <> 0 Then
				If num = 1 Then
					MyBase.animator.Play("SunPink", 0, 0.25F)
				End If
			Else
				MyBase.animator.Play("SwordPink", 0, 0.25F)
			End If
		Else
			Dim num2 As Integer = Global.UnityEngine.Random.Range(0, 3)
			If num2 <> 0 Then
				If num2 <> 1 Then
					If num2 = 2 Then
						MyBase.animator.Play("Moon", 0, 0.25F)
					End If
				Else
					MyBase.animator.Play("Sun", 0, 0.25F)
				End If
			Else
				MyBase.animator.Play("Sword", 0, 0.25F)
			End If
		End If
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06002E9E RID: 11934 RVA: 0x001B7D84 File Offset: 0x001B6184
	Private Iterator Function dead_cr() As IEnumerator
		Me.Speed = 0F
		Me.move = False
		Me.boxCollider.enabled = False
		Me.SFX_SNOWCULT_TarotCardHitGround()
		Select Case CInt((MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F * 10F))
			Case 0, 5
				MyBase.animator.Play("Die")
			Case 1, 6
				MyBase.animator.Play("DieAngleB")
				MyBase.GetComponent(Of SpriteRenderer)().flipX = True
			Case 2, 7
				MyBase.animator.Play("DieAngleA")
				MyBase.GetComponent(Of SpriteRenderer)().flipX = True
			Case 3, 8
				MyBase.animator.Play("DieAngleA")
			Case 4, 9
				MyBase.animator.Play("DieAngleB")
		End Select
		MyBase.animator.Update(0F)
		Dim impactPos As Vector3 = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground))
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99F
			Dim offset As Vector3 = MathUtils.AngleToDirection(CSng(Global.UnityEngine.Random.Range(0, 360))) * MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 250F
			offset.y *= 0.3F
			Me.sparkleEffect.Create(impactPos + offset)
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.005F, 0.01F))
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06002E9F RID: 11935 RVA: 0x001B7DA0 File Offset: 0x001B61A0
	Protected Overrides Sub Move()
		MyBase.transform.position += MyBase.transform.up * -Me.Speed * CupheadTime.FixedDelta - New Vector3(0F, Me._accumulativeGravity * CupheadTime.FixedDelta, 0F)
		If MyBase.transform.position.y <= CSng(Level.Current.Ground) + 15F OrElse MyBase.transform.position.x < CSng(Level.Current.Left) OrElse MyBase.transform.position.x > CSng(Level.Current.Right) Then
			MyBase.StartCoroutine(Me.dead_cr())
		End If
	End Sub

	' Token: 0x06002EA0 RID: 11936 RVA: 0x001B7E80 File Offset: 0x001B6280
	Public Overrides Sub OnParryDie()
		AudioManager.[Stop]("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop")
		MyBase.OnParryDie()
	End Sub

	' Token: 0x06002EA1 RID: 11937 RVA: 0x001B7E92 File Offset: 0x001B6292
	Private Sub SFX_SNOWCULT_TarotCardTravelLoop()
		AudioManager.PlayLoop("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop")
	End Sub

	' Token: 0x06002EA2 RID: 11938 RVA: 0x001B7EAE File Offset: 0x001B62AE
	Private Sub SFX_SNOWCULT_TarotCardHitGround()
		AudioManager.[Stop]("sfx_dlc_snowcult_p1_wizard_tarotcardattack_travel_loop")
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_tarotcard_hitground")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcard_hitground")
	End Sub

	' Token: 0x0400373B RID: 14139
	Private Const GROUND_OFFSET As Single = 15F

	' Token: 0x0400373C RID: 14140
	<SerializeField()>
	Private boxCollider As BoxCollider2D

	' Token: 0x0400373D RID: 14141
	<SerializeField()>
	Private sparkleEffect As Effect
End Class
