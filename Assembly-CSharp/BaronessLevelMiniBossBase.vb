Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020004F5 RID: 1269
Public Class BaronessLevelMiniBossBase
	Inherits AbstractCollidableObject

	' Token: 0x1400003F RID: 63
	' (add) Token: 0x06001646 RID: 5702 RVA: 0x000C3390 File Offset: 0x000C1790
	' (remove) Token: 0x06001647 RID: 5703 RVA: 0x000C33C8 File Offset: 0x000C17C8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As BaronessLevelMiniBossBase.OnDamageTakenHandler

	' Token: 0x06001648 RID: 5704 RVA: 0x000C3400 File Offset: 0x000C1800
	Protected Overridable Sub Start()
		Me.endColor = MyBase.GetComponent(Of SpriteRenderer)().color
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(0F, 0F, 0F, 1F)
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 150
		MyBase.StartCoroutine(Me.switchLayer_cr(Me.layerSwitch))
	End Sub

	' Token: 0x06001649 RID: 5705 RVA: 0x000C3489 File Offset: 0x000C1889
	Protected Overridable Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
	End Sub

	' Token: 0x0600164A RID: 5706 RVA: 0x000C34A8 File Offset: 0x000C18A8
	Protected Overridable Iterator Function switchLayer_cr(layerswitch As Integer) As IEnumerator
		MyBase.StartCoroutine(Me.fade_color_cr())
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Enemies.ToString()
		MyBase.gameObject.GetComponent(Of SpriteRenderer)().sortingOrder = 260
		Return
	End Function

	' Token: 0x0600164B RID: 5707 RVA: 0x000C34C4 File Offset: 0x000C18C4
	Protected Overridable Iterator Function fade_color_cr() As IEnumerator
		Dim t As Single = 0F
		Dim start As Color = New Color(0F, 0F, 0F, 1F)
		While t < Me.fadeTime
			MyBase.GetComponent(Of SpriteRenderer)().color = Color.Lerp(start, Me.endColor, t / Me.fadeTime)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().color = Me.endColor
		Yield Nothing
		Return
	End Function

	' Token: 0x0600164C RID: 5708 RVA: 0x000C34DF File Offset: 0x000C18DF
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x0600164D RID: 5709 RVA: 0x000C3500 File Offset: 0x000C1900
	Protected Overridable Sub StartExplosions()
		If MyBase.GetComponent(Of LevelBossDeathExploder)() IsNot Nothing Then
			MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		End If
	End Sub

	' Token: 0x0600164E RID: 5710 RVA: 0x000C351E File Offset: 0x000C191E
	Protected Overridable Sub EndExplosions()
		If MyBase.GetComponent(Of LevelBossDeathExploder)() IsNot Nothing Then
			MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		End If
	End Sub

	' Token: 0x0600164F RID: 5711 RVA: 0x000C353C File Offset: 0x000C193C
	Protected Overridable Sub Die()
		Me.EndExplosions()
		Me.StopAllCoroutines()
		If MyBase.GetComponent(Of Collider2D)() IsNot Nothing Then
			MyBase.GetComponent(Of Collider2D)().enabled = False
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04001F92 RID: 8082
	Public isDying As Boolean

	' Token: 0x04001F93 RID: 8083
	Public startInvisible As Boolean

	' Token: 0x04001F94 RID: 8084
	Public layerSwitch As Integer = 4

	' Token: 0x04001F95 RID: 8085
	Public bossId As BaronessLevelCastle.BossPossibility

	' Token: 0x04001F97 RID: 8087
	Protected fadeTime As Single = 0.5F

	' Token: 0x04001F98 RID: 8088
	Protected endColor As Color

	' Token: 0x020004F6 RID: 1270
	' (Invoke) Token: 0x06001651 RID: 5713
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
