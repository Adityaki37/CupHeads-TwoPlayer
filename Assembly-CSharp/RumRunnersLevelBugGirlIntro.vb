Imports System
Imports UnityEngine

' Token: 0x02000788 RID: 1928
Public Class RumRunnersLevelBugGirlIntro
	Inherits MonoBehaviour

	' Token: 0x06002A8B RID: 10891 RVA: 0x0018DD47 File Offset: 0x0018C147
	Private Sub OnEnable()
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002A8C RID: 10892 RVA: 0x0018DD60 File Offset: 0x0018C160
	Private Sub OnDisable()
		RemoveHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002A8D RID: 10893 RVA: 0x0018DD79 File Offset: 0x0018C179
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.introAnimation.bugGirlDamage += info.damage
	End Sub

	' Token: 0x06002A8E RID: 10894 RVA: 0x0018DD93 File Offset: 0x0018C193
	Private Sub animationEvent_BugWalkBegin()
		Me.introAnimation.StartBugWalk()
	End Sub

	' Token: 0x06002A8F RID: 10895 RVA: 0x0018DDA0 File Offset: 0x0018C1A0
	Private Sub animationEvent_BugTauntBegin()
		Me.introAnimation.StopBugWalk()
	End Sub

	' Token: 0x06002A90 RID: 10896 RVA: 0x0018DDAD File Offset: 0x0018C1AD
	Private Sub animationEvent_TauntBump()
		Me.introAnimation.BarrelExit()
	End Sub

	' Token: 0x04003357 RID: 13143
	<SerializeField()>
	Private introAnimation As RumRunnersLevelMobIntroAnimation
End Class
