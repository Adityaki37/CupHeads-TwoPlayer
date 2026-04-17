Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000601 RID: 1537
Public Class DragonLevelTail
	Inherits LevelProperties.Dragon.Entity

	' Token: 0x17000378 RID: 888
	' (get) Token: 0x06001E8F RID: 7823 RVA: 0x00119986 File Offset: 0x00117D86
	' (set) Token: 0x06001E90 RID: 7824 RVA: 0x0011998E File Offset: 0x00117D8E
	Public Property state As DragonLevelTail.State

	' Token: 0x06001E91 RID: 7825 RVA: 0x00119998 File Offset: 0x00117D98
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.RegisterCollisionChild(Me.childCollider)
		MyBase.transform.SetPosition(Nothing, New Single?(-1210F), Nothing)
		Me.damageDealer = New DamageDealer(1F, 0.1F, True, False, False)
	End Sub

	' Token: 0x06001E92 RID: 7826 RVA: 0x001199F6 File Offset: 0x00117DF6
	Public Overrides Sub LevelInit(properties As LevelProperties.Dragon)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001E93 RID: 7827 RVA: 0x001199FF File Offset: 0x00117DFF
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001E94 RID: 7828 RVA: 0x00119A17 File Offset: 0x00117E17
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001E95 RID: 7829 RVA: 0x00119A40 File Offset: 0x00117E40
	Public Sub TailStart(warningTime As Single, inTime As Single, holdTime As Single, outTime As Single)
		MyBase.StartCoroutine(Me.go_cr(warningTime, inTime, holdTime, outTime))
	End Sub

	' Token: 0x06001E96 RID: 7830 RVA: 0x00119A54 File Offset: 0x00117E54
	Private Iterator Function go_cr(warningTime As Single, inTime As Single, holdTime As Single, outTime As Single) As IEnumerator
		Me.state = DragonLevelTail.State.Tail
		MyBase.transform.SetPosition(New Single?(PlayerManager.GetNext().transform.position.x), Nothing, Nothing)
		AudioManager.Play("level_dragon_left_dragon_tail_appear")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_tail_appear")
		Yield MyBase.TweenPositionY(-1210F, -1045F, 0.3F, EaseUtils.EaseType.easeOutSine)
		Yield CupheadTime.WaitForSeconds(Me, warningTime)
		AudioManager.Play("level_dragon_left_dragon_tail_attack")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_tail_attack")
		Yield MyBase.TweenPositionY(-1045F, -465F, inTime, EaseUtils.EaseType.easeInSine)
		CupheadLevelCamera.Current.Shake(20F, 0.4F, False)
		Yield CupheadTime.WaitForSeconds(Me, holdTime)
		Yield MyBase.TweenPositionY(-465F, -1210F, outTime, EaseUtils.EaseType.easeInSine)
		Me.state = DragonLevelTail.State.Idle
		Return
	End Function

	' Token: 0x04002766 RID: 10086
	Public Const OUT_Y As Single = -1210F

	' Token: 0x04002767 RID: 10087
	Public Const IN_Y As Single = -465F

	' Token: 0x04002768 RID: 10088
	Public Const START_Y As Single = -1045F

	' Token: 0x04002769 RID: 10089
	Public Const START_TIME As Single = 0.3F

	' Token: 0x0400276B RID: 10091
	<SerializeField()>
	Private childCollider As CollisionChild

	' Token: 0x0400276C RID: 10092
	Private damageDealer As DamageDealer

	' Token: 0x02000602 RID: 1538
	Public Enum State
		' Token: 0x0400276E RID: 10094
		Idle
		' Token: 0x0400276F RID: 10095
		Tail
	End Enum
End Class
