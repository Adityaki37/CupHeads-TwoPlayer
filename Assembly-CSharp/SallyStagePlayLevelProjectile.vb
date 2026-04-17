Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007B3 RID: 1971
Public Class SallyStagePlayLevelProjectile
	Inherits AbstractCollidableObject

	' Token: 0x06002C5F RID: 11359 RVA: 0x001A154F File Offset: 0x0019F94F
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.Awake()
	End Sub

	' Token: 0x06002C60 RID: 11360 RVA: 0x001A1564 File Offset: 0x0019F964
	Private Sub Update()
		Me.sprite.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002C61 RID: 11361 RVA: 0x001A15B0 File Offset: 0x0019F9B0
	Public Sub Init(pos As Vector2, rotation As Single, properties As LevelProperties.SallyStagePlay.Projectile)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.speed = properties.projectileSpeed
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(rotation))
		MyBase.StartCoroutine(Me.move_cr())
		AudioManager.Play("sally_fan_shoot")
		Me.emitAudioFromObject.Add("sally_fan_shoot")
	End Sub

	' Token: 0x06002C62 RID: 11362 RVA: 0x001A162C File Offset: 0x0019FA2C
	Private Iterator Function move_cr() As IEnumerator
		AudioManager.PlayLoop("sally_fan_shoot_loop")
		Me.emitAudioFromObject.Add("sally_fan_shoot_loop")
		While MyBase.transform.position.y > CSng(Level.Current.Ground)
			MyBase.transform.position += MyBase.transform.right * Me.speed * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("OnLand")
		AudioManager.Play("sally_fan_stick")
		Me.emitAudioFromObject.Add("sally_fan_stick")
		AudioManager.[Stop]("sally_fan_shoot_loop")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.groundDuration)
		MyBase.animator.SetTrigger("OnDeath")
		Return
	End Function

	' Token: 0x06002C63 RID: 11363 RVA: 0x001A1647 File Offset: 0x0019FA47
	Private Sub Die()
		Me.StopAllCoroutines()
		AudioManager.Play("sally_fan_dissappear")
		Me.emitAudioFromObject.Add("sally_fan_dissappear")
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002C64 RID: 11364 RVA: 0x001A1674 File Offset: 0x0019FA74
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x040034F8 RID: 13560
	<SerializeField()>
	Private sprite As Transform

	' Token: 0x040034F9 RID: 13561
	Private speed As Single

	' Token: 0x040034FA RID: 13562
	Private damageDealer As DamageDealer

	' Token: 0x040034FB RID: 13563
	Private properties As LevelProperties.SallyStagePlay.Projectile
End Class
