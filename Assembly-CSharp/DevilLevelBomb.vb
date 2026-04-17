Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000589 RID: 1417
Public Class DevilLevelBomb
	Inherits AbstractProjectile

	' Token: 0x06001B0D RID: 6925 RVA: 0x000F8B68 File Offset: 0x000F6F68
	Public Function Create(pos As Vector2, properties As LevelProperties.Devil.BombEye, onLeft As Boolean) As DevilLevelBomb
		Dim devilLevelBomb As DevilLevelBomb = Me.InstantiatePrefab(Of DevilLevelBomb)()
		devilLevelBomb.properties = properties
		devilLevelBomb.transform.position = pos
		devilLevelBomb.startPos = pos
		devilLevelBomb.flipX = Rand.Bool()
		devilLevelBomb.flipY = Rand.Bool()
		devilLevelBomb.onLeft = onLeft
		Return devilLevelBomb
	End Function

	' Token: 0x06001B0E RID: 6926 RVA: 0x000F8BB9 File Offset: 0x000F6FB9
	Protected Overrides Sub Start()
		MyBase.Start()
		AudioManager.Play("p3_bomb_appear")
		Me.emitAudioFromObject.Add("p3_bomb_appear")
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001B0F RID: 6927 RVA: 0x000F8BE8 File Offset: 0x000F6FE8
	Private Iterator Function move_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.5F
		Dim [end] As Single = If((Not Me.onLeft), (Me.startPos.x - 300F), (Me.startPos.x + 300F))
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(Me.startPos.x, [end], t / time)), Nothing, Nothing)
			Yield New WaitForFixedUpdate()
		End While
		MyBase.StartCoroutine(Me.fade_shadow_cr())
		Me.endPos = MyBase.transform.position
		Me.comingOut = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06001B10 RID: 6928 RVA: 0x000F8C04 File Offset: 0x000F7004
	Private Iterator Function fade_shadow_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1F
		While t < time
			t += CupheadTime.Delta
			Me.shadowSprite.color = New Color(1F, 1F, 1F, 1F - t / time)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001B11 RID: 6929 RVA: 0x000F8C20 File Offset: 0x000F7020
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead OrElse Me.comingOut Then
			Return
		End If
		Me.t += CupheadTime.FixedDelta
		If Me.t > Me.properties.explodeDelay Then
			Me.Explode()
			Me.Die()
			Return
		End If
		Dim vector As Vector2 = Me.endPos
		vector.x += Mathf.Sin(Me.t * Me.properties.xSinSpeed * CSng(If((Not Me.flipX), 1, (-1)))) * Me.properties.xSinHeight
		vector.y += Mathf.Sin(Me.t * Me.properties.ySinSpeed * CSng(If((Not Me.flipY), 1, (-1)))) * Me.properties.ySinHeight
		MyBase.transform.SetPosition(New Single?(vector.x), New Single?(vector.y), Nothing)
	End Sub

	' Token: 0x06001B12 RID: 6930 RVA: 0x000F8D3C File Offset: 0x000F713C
	Private Sub Explode()
		Me.explosionPrefab.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06001B13 RID: 6931 RVA: 0x000F8D55 File Offset: 0x000F7155
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001B14 RID: 6932 RVA: 0x000F8D68 File Offset: 0x000F7168
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.explosionPrefab = Nothing
	End Sub

	' Token: 0x0400244B RID: 9291
	<SerializeField()>
	Private explosionPrefab As DevilLevelBombExplosion

	' Token: 0x0400244C RID: 9292
	<SerializeField()>
	Private shadowSprite As SpriteRenderer

	' Token: 0x0400244D RID: 9293
	Private properties As LevelProperties.Devil.BombEye

	' Token: 0x0400244E RID: 9294
	Private startPos As Vector2

	' Token: 0x0400244F RID: 9295
	Private endPos As Vector2

	' Token: 0x04002450 RID: 9296
	Private t As Single

	' Token: 0x04002451 RID: 9297
	Private flipX As Boolean

	' Token: 0x04002452 RID: 9298
	Private flipY As Boolean

	' Token: 0x04002453 RID: 9299
	Private onLeft As Boolean

	' Token: 0x04002454 RID: 9300
	Private comingOut As Boolean = True
End Class
