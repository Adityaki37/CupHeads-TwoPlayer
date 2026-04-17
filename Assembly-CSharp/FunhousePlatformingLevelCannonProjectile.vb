Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B0 RID: 2224
Public Class FunhousePlatformingLevelCannonProjectile
	Inherits BasicProjectile

	' Token: 0x17000449 RID: 1097
	' (get) Token: 0x060033D3 RID: 13267 RVA: 0x001E14D9 File Offset: 0x001DF8D9
	' (set) Token: 0x060033D4 RID: 13268 RVA: 0x001E14E1 File Offset: 0x001DF8E1
	Public Property Properties As EnemyProperties

	' Token: 0x060033D5 RID: 13269 RVA: 0x001E14EA File Offset: 0x001DF8EA
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.animator.Play("anim_level_starcannon_bullet", -1, Global.UnityEngine.Random.value)
	End Sub

	' Token: 0x060033D6 RID: 13270 RVA: 0x001E1508 File Offset: 0x001DF908
	Public Sub Init()
		MyBase.StartCoroutine(Me.delayedDeath_cr())
	End Sub

	' Token: 0x060033D7 RID: 13271 RVA: 0x001E1518 File Offset: 0x001DF918
	Private Iterator Function delayedDeath_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.Properties.bulletDeathTime)
		Me.Die()
		Return
	End Function

	' Token: 0x060033D8 RID: 13272 RVA: 0x001E1534 File Offset: 0x001DF934
	Protected Overrides Sub Die()
		MyBase.Die()
		Dim effect As Effect = Me.deathFx.Create(MyBase.transform.position, New Vector3(1.25F, 1.25F, 1F))
		effect.animator.SetInteger("PickAni", Global.UnityEngine.Random.Range(0, 3))
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060033D9 RID: 13273 RVA: 0x001E1594 File Offset: 0x001DF994
	Protected Overrides Sub Move()
		If Me.Speed = 0F Then
		End If
		MyBase.transform.position += Me.direction * Me.Speed * CupheadTime.FixedDelta - New Vector3(0F, Me._accumulativeGravity * CupheadTime.FixedDelta, 0F)
		Me._accumulativeGravity += Me.Gravity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x060033DA RID: 13274 RVA: 0x001E161B File Offset: 0x001DFA1B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.deathFx = Nothing
	End Sub

	' Token: 0x04003C20 RID: 15392
	<SerializeField()>
	Private deathFx As Effect

	' Token: 0x04003C22 RID: 15394
	Public direction As Vector3
End Class
