Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A50 RID: 2640
Public Class PlayerSuperChaliceBounce
	Inherits AbstractPlayerSuper

	' Token: 0x06003EE5 RID: 16101 RVA: 0x00226F34 File Offset: 0x00225334
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x06003EE6 RID: 16102 RVA: 0x00226F3C File Offset: 0x0022533C
	Protected Overrides Sub StartSuper()
		MyBase.StartSuper()
		MyBase.StartCoroutine(Me.super_cr())
	End Sub

	' Token: 0x06003EE7 RID: 16103 RVA: 0x00226F54 File Offset: 0x00225354
	Private Iterator Function super_cr() As IEnumerator
		Dim duration As Single = Me.DURATION
		Me.timer = duration
		Me.Fire()
		If Me.LAUNCHED_VERSION Then
			Yield New WaitForEndOfFrame()
			Me.player.animationController.EnableSpriteRenderer()
		End If
		While Me.timer > 0F AndAlso Not Me.interrupted
			Me.timer -= CupheadTime.FixedDelta
			Yield Nothing
		End While
		If Not Me.LAUNCHED_VERSION Then
			Me.EndSuper(True)
			Me.player.transform.position = Me.ball.transform.position
		End If
		Me.CleanUp()
		Return
	End Function

	' Token: 0x06003EE8 RID: 16104 RVA: 0x00226F6F File Offset: 0x0022536F
	Public Sub CleanUp()
		Global.UnityEngine.[Object].Destroy(Me.ball.gameObject)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003EE9 RID: 16105 RVA: 0x00226F8C File Offset: 0x0022538C
	Protected Overrides Sub Fire()
		PauseManager.Unpause()
		If Not Me.LAUNCHED_VERSION Then
			Me.player.PauseAll()
			Dim component As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
			component.IgnoreGlobal = False
		Else
			Me.EndSuper(True)
			Me.player.stats.OnSuperEnd()
		End If
		Me.ball = TryCast(Me.ball.Create(MyBase.transform.position + Vector3.up * 100F), PlayerSuperChaliceBounceBall)
		Me.ball.player = Me.player
		Me.ball.PlayerId = Me.player.id
		Me.ball.velocity.x = CSng((Me.player.motor.MoveDirection.x * 500))
		Me.ball.Damage = Me.DAMAGE
		Me.ball.DamageRate = Me.DAMAGE_RATE
		Me.ball.super = Me
	End Sub

	' Token: 0x040045DF RID: 17887
	<NonSerialized()>
	Public LAUNCHED_VERSION As Boolean = WeaponProperties.LevelSuperChaliceBounce.launchedVersion

	' Token: 0x040045E0 RID: 17888
	Private DAMAGE As Single = WeaponProperties.LevelSuperChaliceBounce.damage

	' Token: 0x040045E1 RID: 17889
	Private DAMAGE_RATE As Single = WeaponProperties.LevelSuperChaliceBounce.damageRate

	' Token: 0x040045E2 RID: 17890
	Private DURATION As Single = WeaponProperties.LevelSuperChaliceBounce.duration

	' Token: 0x040045E3 RID: 17891
	<SerializeField()>
	Private ball As PlayerSuperChaliceBounceBall

	' Token: 0x040045E4 RID: 17892
	Public timer As Single
End Class
