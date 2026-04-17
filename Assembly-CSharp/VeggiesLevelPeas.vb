Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000853 RID: 2131
Public Class VeggiesLevelPeas
	Inherits LevelProperties.Veggies.Entity

	' Token: 0x1700042B RID: 1067
	' (get) Token: 0x06003168 RID: 12648 RVA: 0x001CE50A File Offset: 0x001CC90A
	' (set) Token: 0x06003169 RID: 12649 RVA: 0x001CE512 File Offset: 0x001CC912
	Public Property state As VeggiesLevelPeas.State

	' Token: 0x1400005F RID: 95
	' (add) Token: 0x0600316A RID: 12650 RVA: 0x001CE51C File Offset: 0x001CC91C
	' (remove) Token: 0x0600316B RID: 12651 RVA: 0x001CE554 File Offset: 0x001CC954
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As VeggiesLevelPeas.OnDamageTakenHandler

	' Token: 0x0600316C RID: 12652 RVA: 0x001CE58A File Offset: 0x001CC98A
	Private Sub Start()
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x0600316D RID: 12653 RVA: 0x001CE598 File Offset: 0x001CC998
	Public Overrides Sub LevelInitWithGroup(propertyGroup As AbstractLevelPropertyGroup)
		MyBase.LevelInitWithGroup(propertyGroup)
		Me.properties = TryCast(propertyGroup, LevelProperties.Veggies.Peas)
		Me.hp = CSng(Me.properties.hp)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600316E RID: 12654 RVA: 0x001CE5D8 File Offset: 0x001CC9D8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600316F RID: 12655 RVA: 0x001CE62A File Offset: 0x001CCA2A
	Private Sub OnInAnimComplete()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.peas_cr())
	End Sub

	' Token: 0x06003170 RID: 12656 RVA: 0x001CE645 File Offset: 0x001CCA45
	Private Sub OnDeathAnimComplete()
		Me.state = VeggiesLevelPeas.State.Complete
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003171 RID: 12657 RVA: 0x001CE659 File Offset: 0x001CCA59
	Private Sub Die()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x06003172 RID: 12658 RVA: 0x001CE670 File Offset: 0x001CCA70
	Private Iterator Function peas_cr() As IEnumerator
		Yield Nothing
		Return
	End Function

	' Token: 0x06003173 RID: 12659 RVA: 0x001CE684 File Offset: 0x001CCA84
	Private Iterator Function die_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("Idle")
		Yield MyBase.StartCoroutine(MyBase.dieFlash_cr())
		MyBase.animator.SetTrigger("Dead")
		Return
	End Function

	' Token: 0x040039F0 RID: 14832
	<SerializeField()>
	Private projectilePrefab As VeggiesLevelOnionTearProjectile

	' Token: 0x040039F1 RID: 14833
	Private properties As LevelProperties.Veggies.Peas

	' Token: 0x040039F2 RID: 14834
	Private hp As Single

	' Token: 0x02000854 RID: 2132
	Public Enum State
		' Token: 0x040039F5 RID: 14837
		Start
		' Token: 0x040039F6 RID: 14838
		Complete
	End Enum

	' Token: 0x02000855 RID: 2133
	' (Invoke) Token: 0x06003175 RID: 12661
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
