Imports System
Imports UnityEngine

' Token: 0x0200072D RID: 1837
Public Class PirateLevelSquidProjectile
	Inherits AbstractMonoBehaviour

	' Token: 0x06002803 RID: 10243 RVA: 0x00176208 File Offset: 0x00174608
	Private Sub Update()
		If Me.state = PirateLevelSquidProjectile.State.Moving Then
			If Me.lifetime > 5F Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
				Return
			End If
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.Delta
		End If
		Me.lifetime += CupheadTime.Delta
	End Sub

	' Token: 0x06002804 RID: 10244 RVA: 0x001762B4 File Offset: 0x001746B4
	Private Sub OnTriggerEnter2D(collider As Collider2D)
		If collider.name = PlayerId.PlayerOne.ToString() OrElse collider.name = PlayerId.PlayerTwo.ToString() Then
			PirateLevelSquidInkOverlay.Current.Hit()
			CupheadLevelCamera.Current.Shake(4F, 0.3F, False)
			Me.Die()
		ElseIf collider.name = "Level_Ground" Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002805 RID: 10245 RVA: 0x00176344 File Offset: 0x00174744
	Public Sub Create(pos As Vector2, velocity As Vector2, gravity As Single)
		Me.InstantiatePrefab(Of PirateLevelSquidProjectile)().Init(pos, velocity, gravity)
	End Sub

	' Token: 0x06002806 RID: 10246 RVA: 0x00176354 File Offset: 0x00174754
	Private Sub Init(pos As Vector2, velocity As Vector2, gravity As Single)
		MyBase.transform.position = pos
		Me.velocity = velocity
		Me.gravity = gravity
		Me.state = PirateLevelSquidProjectile.State.Moving
	End Sub

	' Token: 0x06002807 RID: 10247 RVA: 0x0017637C File Offset: 0x0017477C
	Private Sub Die()
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.state = PirateLevelSquidProjectile.State.Dead
	End Sub

	' Token: 0x06002808 RID: 10248 RVA: 0x001763A1 File Offset: 0x001747A1
	Private Sub OnDeathAnimationComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040030C6 RID: 12486
	Public Const MAX_LIFETIME As Single = 5F

	' Token: 0x040030C7 RID: 12487
	Private state As PirateLevelSquidProjectile.State

	' Token: 0x040030C8 RID: 12488
	Private velocity As Vector2

	' Token: 0x040030C9 RID: 12489
	Private gravity As Single

	' Token: 0x040030CA RID: 12490
	Private lifetime As Single

	' Token: 0x0200072E RID: 1838
	Public Enum State
		' Token: 0x040030CC RID: 12492
		Init
		' Token: 0x040030CD RID: 12493
		Moving
		' Token: 0x040030CE RID: 12494
		Dead
	End Enum
End Class
