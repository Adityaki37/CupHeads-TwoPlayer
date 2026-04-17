Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000604 RID: 1540
Public Class FlowerLevelBoomerang
	Inherits BasicProjectile

	' Token: 0x17000379 RID: 889
	' (get) Token: 0x06001E99 RID: 7833 RVA: 0x00119CAE File Offset: 0x001180AE
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x06001E9A RID: 7834 RVA: 0x00119CB4 File Offset: 0x001180B4
	Public Sub OnBoomerangStart(delay As Single)
		Me.BoomerangNumberSFX += 1
		If Me.BoomerangNumberSFX = 1 Then
			AudioManager.FadeSFXVolume("flower_boomerang_1", 1F, 1F)
			AudioManager.PlayLoop("flower_boomerang_1")
			Me.emitAudioFromObject.Add("flower_boomerang_1")
		ElseIf Me.BoomerangNumberSFX <> 1 Then
			AudioManager.FadeSFXVolume("flower_boomerang_2", 1F, 1F)
			AudioManager.PlayLoop("flower_boomerang_2")
			Me.emitAudioFromObject.Add("flower_boomerang_2")
		End If
		Me.offScreenDelay = delay
		Me.returnXPosition = CSng((Level.Current.Left - 100))
		Me.endXPosition = CSng((Level.Current.Right + 500))
		MyBase.StartCoroutine(Me.boomerangStart_cr())
	End Sub

	' Token: 0x06001E9B RID: 7835 RVA: 0x00119D88 File Offset: 0x00118188
	Private Iterator Function boomerangStart_cr() As IEnumerator
		MyBase.transform.GetChild(0).transform.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
		While MyBase.transform.position.x > Me.returnXPosition
			Yield Nothing
		End While
		Me.move = False
		Yield CupheadTime.WaitForSeconds(Me, Me.offScreenDelay)
		Me.OnBoomerangReturn()
		Return
	End Function

	' Token: 0x06001E9C RID: 7836 RVA: 0x00119DA4 File Offset: 0x001181A4
	Private Sub OnBoomerangReturn()
		Me.Speed = -Me.Speed
		Me.move = True
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, CSng((Level.Current.Ground + Level.Current.Height / 6)), 0F)
		MyBase.StartCoroutine(Me.boomerangReturn_cr())
	End Sub

	' Token: 0x06001E9D RID: 7837 RVA: 0x00119E14 File Offset: 0x00118214
	Private Iterator Function boomerangReturn_cr() As IEnumerator
		MyBase.transform.GetChild(0).transform.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground), 0F)
		While MyBase.transform.position.x < Me.endXPosition
			Yield Nothing
		End While
		If Me.BoomerangNumberSFX = 1 Then
			AudioManager.FadeSFXVolume("flower_boomerang_1", 0F, 3F)
			AudioManager.FadeSFXVolume("flower_boomerang_2", 0F, 3F)
		ElseIf Me.BoomerangNumberSFX <> 1 Then
			AudioManager.FadeSFXVolume("flower_boomerang_1", 0F, 3F)
			AudioManager.FadeSFXVolume("flower_boomerang_2", 0F, 3F)
		End If
		Me.BoomerangNumberSFX -= 1
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001E9E RID: 7838 RVA: 0x00119E2F File Offset: 0x0011822F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001E9F RID: 7839 RVA: 0x00119E58 File Offset: 0x00118258
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06001EA0 RID: 7840 RVA: 0x00119E60 File Offset: 0x00118260
	Public Overrides Sub OnLevelEnd()
		AudioManager.[Stop]("flower_boomerang_1")
		AudioManager.[Stop]("flower_boomerang_2")
		MyBase.OnLevelEnd()
	End Sub

	' Token: 0x04002770 RID: 10096
	Private returnXPosition As Single

	' Token: 0x04002771 RID: 10097
	Private endXPosition As Single

	' Token: 0x04002772 RID: 10098
	Private offScreenDelay As Single

	' Token: 0x04002773 RID: 10099
	Private BoomerangNumberSFX As Integer
End Class
