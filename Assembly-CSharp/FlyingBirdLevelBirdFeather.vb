Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200061A RID: 1562
Public Class FlyingBirdLevelBirdFeather
	Inherits AbstractProjectile

	' Token: 0x1700037C RID: 892
	' (get) Token: 0x06001FBB RID: 8123 RVA: 0x00123B3B File Offset: 0x00121F3B
	' (set) Token: 0x06001FBC RID: 8124 RVA: 0x00123B43 File Offset: 0x00121F43
	Public Property Speed As Single

	' Token: 0x06001FBD RID: 8125 RVA: 0x00123B4C File Offset: 0x00121F4C
	Public Overridable Function Init(speed As Single) As AbstractProjectile
		Me.Speed = speed
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Return Me
	End Function

	' Token: 0x06001FBE RID: 8126 RVA: 0x00123B64 File Offset: 0x00121F64
	Protected Overrides Sub Update()
		MyBase.Update()
		MyBase.transform.position += -MyBase.transform.right * Me.Speed * CupheadTime.Delta
	End Sub

	' Token: 0x06001FBF RID: 8127 RVA: 0x00123BB7 File Offset: 0x00121FB7
	Private Sub OnEnable()
		Me.DamagesType.OnlyPlayer()
		Me.CollisionDeath.OnlyPlayer()
		Me.SetCollider(True)
	End Sub

	' Token: 0x06001FC0 RID: 8128 RVA: 0x00123BD7 File Offset: 0x00121FD7
	Private Sub OnDisable()
		Me.SetCollider(False)
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x06001FC1 RID: 8129 RVA: 0x00123BE6 File Offset: 0x00121FE6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001FC2 RID: 8130 RVA: 0x00123C04 File Offset: 0x00122004
	Private Sub SetCollider(c As Boolean)
		MyBase.GetComponent(Of BoxCollider2D)().enabled = c
	End Sub

	' Token: 0x06001FC3 RID: 8131 RVA: 0x00123C14 File Offset: 0x00122014
	Private Iterator Function effect_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.3F))
		While True
			Me.effectPrefab.Create(Me.effectRoot.position)
			Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		End While
		Return
	End Function

	' Token: 0x06001FC4 RID: 8132 RVA: 0x00123C2F File Offset: 0x0012202F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.effectPrefab = Nothing
	End Sub

	' Token: 0x06001FC5 RID: 8133 RVA: 0x00123C3E File Offset: 0x0012203E
	Public Overrides Sub OnParryDie()
		Me.Recycle()
	End Sub

	' Token: 0x06001FC6 RID: 8134 RVA: 0x00123C46 File Offset: 0x00122046
	Protected Overrides Sub OnDieDistance()
		Me.Recycle()
	End Sub

	' Token: 0x06001FC7 RID: 8135 RVA: 0x00123C4E File Offset: 0x0012204E
	Protected Overrides Sub OnDieLifetime()
		Me.Recycle()
	End Sub

	' Token: 0x06001FC8 RID: 8136 RVA: 0x00123C56 File Offset: 0x00122056
	Protected Overrides Sub OnDieAnimationComplete()
		Me.Recycle()
	End Sub

	' Token: 0x04002848 RID: 10312
	<SerializeField()>
	Private effectPrefab As Effect

	' Token: 0x04002849 RID: 10313
	<SerializeField()>
	Private effectRoot As Transform
End Class
