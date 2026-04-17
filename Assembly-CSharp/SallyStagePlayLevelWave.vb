Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007BB RID: 1979
Public Class SallyStagePlayLevelWave
	Inherits AbstractCollidableObject

	' Token: 0x1700040B RID: 1035
	' (get) Token: 0x06002CBC RID: 11452 RVA: 0x001A6262 File Offset: 0x001A4662
	' (set) Token: 0x06002CBD RID: 11453 RVA: 0x001A626A File Offset: 0x001A466A
	Public Property isMoving As Boolean

	' Token: 0x06002CBE RID: 11454 RVA: 0x001A6273 File Offset: 0x001A4673
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.startPos = MyBase.transform.position
	End Sub

	' Token: 0x06002CBF RID: 11455 RVA: 0x001A6291 File Offset: 0x001A4691
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002CC0 RID: 11456 RVA: 0x001A62A9 File Offset: 0x001A46A9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002CC1 RID: 11457 RVA: 0x001A62C7 File Offset: 0x001A46C7
	Public Sub StartWave(properties As LevelProperties.SallyStagePlay.Tidal)
		Me.properties = properties
		MyBase.transform.position = Me.startPos
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002CC2 RID: 11458 RVA: 0x001A62F0 File Offset: 0x001A46F0
	Private Iterator Function move_cr() As IEnumerator
		Dim sizeX As Single = MyBase.GetComponent(Of Renderer)().bounds.size.x
		Me.isMoving = True
		While MyBase.transform.position.x < 640F + sizeX
			MyBase.transform.position += MyBase.transform.right * Me.properties.tidalSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Me.isMoving = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06002CC3 RID: 11459 RVA: 0x001A630B File Offset: 0x001A470B
	Private Sub SoundBigWaveFeet()
		If Me.isMoving Then
			AudioManager.Play("sally_wave")
			Me.emitAudioFromObject.Add("sally_wave")
		End If
	End Sub

	' Token: 0x06002CC4 RID: 11460 RVA: 0x001A6332 File Offset: 0x001A4732
	Private Sub SoundBigWaveVoice()
		If Me.isMoving Then
			AudioManager.Play("sally_wave_sweet")
			Me.emitAudioFromObject.Add("sally_wave_sweet")
		End If
	End Sub

	' Token: 0x0400353D RID: 13629
	Private damageDealer As DamageDealer

	' Token: 0x0400353E RID: 13630
	Private properties As LevelProperties.SallyStagePlay.Tidal

	' Token: 0x0400353F RID: 13631
	Private startPos As Vector3
End Class
