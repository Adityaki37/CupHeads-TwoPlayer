Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200059E RID: 1438
Public Class DicePalaceBoozeLevelBossBase
	Inherits LevelProperties.DicePalaceBooze.Entity

	' Token: 0x1700035B RID: 859
	' (get) Token: 0x06001B8F RID: 7055 RVA: 0x000FB6A7 File Offset: 0x000F9AA7
	' (set) Token: 0x06001B90 RID: 7056 RVA: 0x000FB6AF File Offset: 0x000F9AAF
	Public Property isDead As Boolean

	' Token: 0x1700035C RID: 860
	' (get) Token: 0x06001B91 RID: 7057 RVA: 0x000FB6B8 File Offset: 0x000F9AB8
	' (set) Token: 0x06001B92 RID: 7058 RVA: 0x000FB6BF File Offset: 0x000F9ABF
	Public Shared Property DEATH_COUNTER As Integer

	' Token: 0x1700035D RID: 861
	' (get) Token: 0x06001B93 RID: 7059 RVA: 0x000FB6C7 File Offset: 0x000F9AC7
	' (set) Token: 0x06001B94 RID: 7060 RVA: 0x000FB6CE File Offset: 0x000F9ACE
	Public Shared Property ATTACK_DELAY As Single

	' Token: 0x06001B95 RID: 7061 RVA: 0x000FB6D6 File Offset: 0x000F9AD6
	Private Sub Start()
		Me.isDead = False
		DicePalaceBoozeLevelBossBase.DEATH_COUNTER = 0
		DicePalaceBoozeLevelBossBase.ATTACK_DELAY = 0F
	End Sub

	' Token: 0x06001B96 RID: 7062 RVA: 0x000FB6EF File Offset: 0x000F9AEF
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceBooze)
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001B97 RID: 7063 RVA: 0x000FB6F8 File Offset: 0x000F9AF8
	Protected Overridable Sub StartDying()
		Me.isDead = True
		DicePalaceBoozeLevelBossBase.DEATH_COUNTER += 1
		If DicePalaceBoozeLevelBossBase.DEATH_COUNTER >= 3 Then
			Me.AllDead()
		Else
			DicePalaceBoozeLevelBossBase.ATTACK_DELAY += MyBase.properties.CurrentState.main.delaySubstractAmount
			Me.Dying()
		End If
	End Sub

	' Token: 0x06001B98 RID: 7064 RVA: 0x000FB754 File Offset: 0x000F9B54
	Private Sub Dying()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.dying_cr())
	End Sub

	' Token: 0x06001B99 RID: 7065 RVA: 0x000FB76C File Offset: 0x000F9B6C
	Private Iterator Function dying_cr() As IEnumerator
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		Global.UnityEngine.[Object].Destroy(MyBase.GetComponent(Of Rigidbody2D)())
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001B9A RID: 7066 RVA: 0x000FB787 File Offset: 0x000F9B87
	Private Sub AllDead()
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.properties.DealDamageToNextNamedState()
	End Sub

	' Token: 0x06001B9B RID: 7067 RVA: 0x000FB7AA File Offset: 0x000F9BAA
	Protected Overridable Sub HandleDead()
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x040024AD RID: 9389
	Protected health As Single
End Class
