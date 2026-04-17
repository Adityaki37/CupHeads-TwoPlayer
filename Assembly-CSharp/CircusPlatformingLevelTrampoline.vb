Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008AD RID: 2221
Public Class CircusPlatformingLevelTrampoline
	Inherits AbstractCollidableObject

	' Token: 0x17000447 RID: 1095
	' (get) Token: 0x060033C0 RID: 13248 RVA: 0x001E0B40 File Offset: 0x001DEF40
	' (set) Token: 0x060033C1 RID: 13249 RVA: 0x001E0B48 File Offset: 0x001DEF48
	Public Property MoveDirection As CircusPlatformingLevelTrampoline.Direction

	' Token: 0x17000448 RID: 1096
	' (get) Token: 0x060033C2 RID: 13250 RVA: 0x001E0B51 File Offset: 0x001DEF51
	' (set) Token: 0x060033C3 RID: 13251 RVA: 0x001E0B59 File Offset: 0x001DEF59
	Public Property TrackingPlayer As AbstractPlayerController

	' Token: 0x060033C4 RID: 13252 RVA: 0x001E0B62 File Offset: 0x001DEF62
	Private Sub Start()
		Me.startPos = MyBase.transform.position
		MyBase.StartCoroutine(Me.loop_cr())
		MyBase.StartCoroutine(Me.sleep_sfx_cr())
	End Sub

	' Token: 0x060033C5 RID: 13253 RVA: 0x001E0B94 File Offset: 0x001DEF94
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If phase = CollisionPhase.Enter OrElse phase = CollisionPhase.Stay Then
			Dim component As LevelPlayerMotor = hit.GetComponent(Of LevelPlayerMotor)()
			If component IsNot Nothing AndAlso component.Grounded Then
				MyBase.animator.SetTrigger("Bounce")
				component.OnTrampolineKnockUp(Me.knockUpHeight)
			End If
		End If
	End Sub

	' Token: 0x060033C6 RID: 13254 RVA: 0x001E0BF0 File Offset: 0x001DEFF0
	Private Iterator Function loop_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.TrackingPlayer = PlayerManager.GetNext()
		While Me.TrackingPlayer Is Nothing
			Yield Nothing
		End While
		Dim minBounds As Single = MyBase.transform.localPosition.x - Me.bounds
		Dim maxBounds As Single = MyBase.transform.localPosition.x + Me.bounds
		While True
			If Not MyBase.enabled Then
				Yield Nothing
			Else
				If Me.TrackingPlayer Is Nothing Then
					Me.TrackingPlayer = PlayerManager.GetNext()
				End If
				If Me.TrackingPlayer.transform.position.x > MyBase.transform.position.x Then
					Me.MoveDirection = CircusPlatformingLevelTrampoline.Direction.Right
				Else
					Me.MoveDirection = CircusPlatformingLevelTrampoline.Direction.Left
				End If
				If Me.MoveDirection = CircusPlatformingLevelTrampoline.Direction.Right Then
					If MyBase.transform.position.x < Me.startPos.x + Me.bounds Then
						Me.velocityX += Me.acceleration * CupheadTime.Delta
					Else
						Me.velocityX = 0F
					End If
				ElseIf MyBase.transform.position.x > Me.startPos.x - Me.bounds Then
					Me.velocityX -= Me.acceleration * CupheadTime.Delta
				Else
					Me.velocityX = 0F
				End If
				Me.velocityX = Mathf.Clamp(Me.velocityX, -Me.maxSpeed, Me.maxSpeed)
				Me.position = MyBase.transform.localPosition
				Me.position.x = Me.position.x + Me.velocityX * CupheadTime.Delta
				If Me.position.x < minBounds Then
					Me.position.x = minBounds
					Me.velocityX = 0F
				ElseIf Me.position.x > maxBounds Then
					Me.position.x = maxBounds
					Me.velocityX = 0F
				End If
				MyBase.transform.localPosition = Me.position
				If Me.TrackingPlayer.IsDead Then
					Me.TrackingPlayer = PlayerManager.GetNext()
				End If
				Me.CheckIfShouldSleep()
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x060033C7 RID: 13255 RVA: 0x001E0C0C File Offset: 0x001DF00C
	Private Sub CheckIfShouldSleep()
		Dim transform As Transform = PlayerManager.GetPlayer(PlayerId.PlayerOne).transform
		If Me.IsInBounds(transform) Then
			MyBase.animator.SetBool("Sleep", False)
			If PlayerManager.Multiplayer AndAlso Me.IsInBounds(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform) Then
				Me.TrackingPlayer = PlayerManager.GetNext()
			Else
				Me.TrackingPlayer = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			End If
		ElseIf PlayerManager.Multiplayer AndAlso Me.IsInBounds(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform) Then
			MyBase.animator.SetBool("Sleep", False)
			Me.TrackingPlayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		Else
			If MyBase.transform.position.x >= Me.startPos.x + Me.bounds OrElse MyBase.transform.position.x <= Me.startPos.x - Me.bounds Then
				MyBase.animator.SetBool("Sleep", True)
			End If
			Me.TrackingPlayer = PlayerManager.GetNext()
		End If
	End Sub

	' Token: 0x060033C8 RID: 13256 RVA: 0x001E0D38 File Offset: 0x001DF138
	Private Iterator Function sleep_sfx_cr() As IEnumerator
		While True
			If MyBase.animator.GetBool("Sleep") Then
				Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(2F, 5F))
				If MyBase.animator.GetBool("Sleep") Then
					AudioManager.Play("circus_trampoline_sleep_boil")
					Me.emitAudioFromObject.Add("circus_trampoline_sleep_boil")
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060033C9 RID: 13257 RVA: 0x001E0D54 File Offset: 0x001DF154
	Private Function IsInBounds(other As Transform) As Boolean
		Dim num As Single = Me.startPos.x - Me.bounds
		Dim num2 As Single = Me.startPos.x + Me.bounds
		Return other.position.x < num2 + Me.AwakeningZone AndAlso other.position.x > num - Me.AwakeningZone
	End Function

	' Token: 0x060033CA RID: 13258 RVA: 0x001E0DC0 File Offset: 0x001DF1C0
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(New Vector2(Me.startPos.x + Me.bounds, Me.startPos.y), New Vector2(Me.startPos.x - Me.bounds, Me.startPos.y))
	End Sub

	' Token: 0x060033CB RID: 13259 RVA: 0x001E0E28 File Offset: 0x001DF228
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.DrawLine(New Vector2(MyBase.transform.position.x + Me.bounds, MyBase.transform.position.y), New Vector2(MyBase.transform.position.x - Me.bounds, MyBase.transform.position.y))
	End Sub

	' Token: 0x060033CC RID: 13260 RVA: 0x001E0EAE File Offset: 0x001DF2AE
	Private Sub TrampolineBounceSFX()
		AudioManager.Play("circus_trampoline_bounce")
		Me.emitAudioFromObject.Add("circus_trampoline_bounce")
	End Sub

	' Token: 0x060033CD RID: 13261 RVA: 0x001E0ECA File Offset: 0x001DF2CA
	Private Sub TrampolineIntroSFX()
		AudioManager.[Stop]("circus_trampoline_idle_loop")
		AudioManager.Play("circus_trampoline_sleep_intro")
		Me.emitAudioFromObject.Add("circus_trampoline_sleep_intro")
	End Sub

	' Token: 0x060033CE RID: 13262 RVA: 0x001E0EF0 File Offset: 0x001DF2F0
	Private Sub TrampolineOutroSFX()
		AudioManager.Play("circus_trampoline_sleep_outro")
		Me.emitAudioFromObject.Add("circus_trampoline_sleep_outro")
	End Sub

	' Token: 0x060033CF RID: 13263 RVA: 0x001E0F0C File Offset: 0x001DF30C
	Private Sub TrampolineStartIdleSFX()
		AudioManager.PlayLoop("circus_trampoline_idle_loop")
		Me.emitAudioFromObject.Add("circus_trampoline_idle_loop")
	End Sub

	' Token: 0x04003C0F RID: 15375
	Private Const BounceTrigger As String = "Bounce"

	' Token: 0x04003C10 RID: 15376
	Private Const Sleep As String = "Sleep"

	' Token: 0x04003C11 RID: 15377
	<SerializeField()>
	Private bounds As Single

	' Token: 0x04003C12 RID: 15378
	<SerializeField()>
	Private AwakeningZone As Single = 500F

	' Token: 0x04003C13 RID: 15379
	Public maxSpeed As Single

	' Token: 0x04003C14 RID: 15380
	Public acceleration As Single

	' Token: 0x04003C15 RID: 15381
	Public knockUpHeight As Single = -1.95F

	' Token: 0x04003C16 RID: 15382
	Private velocityX As Single

	' Token: 0x04003C17 RID: 15383
	Private startPos As Vector2

	' Token: 0x04003C18 RID: 15384
	Private position As Vector2

	' Token: 0x020008AE RID: 2222
	Public Enum Direction
		' Token: 0x04003C1C RID: 15388
		Left
		' Token: 0x04003C1D RID: 15389
		Right
	End Enum
End Class
