Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000856 RID: 2134
Public Class VeggiesLevelPotato
	Inherits LevelProperties.Veggies.Entity

	' Token: 0x1700042C RID: 1068
	' (get) Token: 0x06003179 RID: 12665 RVA: 0x001CE805 File Offset: 0x001CCC05
	' (set) Token: 0x0600317A RID: 12666 RVA: 0x001CE80D File Offset: 0x001CCC0D
	Public Property state As VeggiesLevelPotato.State

	' Token: 0x14000060 RID: 96
	' (add) Token: 0x0600317B RID: 12667 RVA: 0x001CE818 File Offset: 0x001CCC18
	' (remove) Token: 0x0600317C RID: 12668 RVA: 0x001CE850 File Offset: 0x001CCC50
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As VeggiesLevelPotato.OnDamageTakenHandler

	' Token: 0x0600317D RID: 12669 RVA: 0x001CE886 File Offset: 0x001CCC86
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageDealer.SetDirection(DamageDealer.Direction.Neutral, MyBase.transform)
	End Sub

	' Token: 0x0600317E RID: 12670 RVA: 0x001CE8AB File Offset: 0x001CCCAB
	Private Sub Start()
		Me.SfxGround()
	End Sub

	' Token: 0x0600317F RID: 12671 RVA: 0x001CE8B4 File Offset: 0x001CCCB4
	Public Overrides Sub LevelInitWithGroup(propertyGroup As AbstractLevelPropertyGroup)
		MyBase.LevelInitWithGroup(propertyGroup)
		Me.properties = TryCast(propertyGroup, LevelProperties.Veggies.Potato)
		Me.hp = CSng(Me.properties.hp)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.potato_cr())
	End Sub

	' Token: 0x06003180 RID: 12672 RVA: 0x001CE90A File Offset: 0x001CCD0A
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003181 RID: 12673 RVA: 0x001CE924 File Offset: 0x001CCD24
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06003182 RID: 12674 RVA: 0x001CE976 File Offset: 0x001CCD76
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003183 RID: 12675 RVA: 0x001CE994 File Offset: 0x001CCD94
	Private Sub Die()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("Dead")
	End Sub

	' Token: 0x06003184 RID: 12676 RVA: 0x001CE9B8 File Offset: 0x001CCDB8
	Private Sub StartExplosions()
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
	End Sub

	' Token: 0x06003185 RID: 12677 RVA: 0x001CE9C5 File Offset: 0x001CCDC5
	Private Sub EndExplosions()
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
	End Sub

	' Token: 0x06003186 RID: 12678 RVA: 0x001CE9D2 File Offset: 0x001CCDD2
	Private Sub SfxGround()
		AudioManager.Play("level_veggies_potato_ground")
	End Sub

	' Token: 0x06003187 RID: 12679 RVA: 0x001CE9DE File Offset: 0x001CCDDE
	Private Sub OnInAnimComplete()
	End Sub

	' Token: 0x06003188 RID: 12680 RVA: 0x001CE9E0 File Offset: 0x001CCDE0
	Private Sub OnDeathAnimComplete()
		Me.state = VeggiesLevelPotato.State.Complete
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003189 RID: 12681 RVA: 0x001CE9F4 File Offset: 0x001CCDF4
	Private Sub Shoot()
		If Not Me.projectileParryFlag Then
			AudioManager.Play("levels_veggies_potato_spit")
		Else
			AudioManager.Play("level_veggies_potato_spit_worm")
		End If
		Me.didShoot = True
		Dim basicProjectile As BasicProjectile = Me.projectilePrefab.Create(Me.gunRoot.position, Me.gunRoot.eulerAngles.z, Me.properties.bulletSpeed)
		basicProjectile.SetParryable(Me.projectileParryFlag)
		Me.spitEffect.Create(Me.gunRoot.position)
	End Sub

	' Token: 0x0600318A RID: 12682 RVA: 0x001CEA8C File Offset: 0x001CCE8C
	Private Iterator Function potato_cr() As IEnumerator
		While True
			Dim groups As Integer = 0
			Dim shots As Integer = 0
			While groups < Me.properties.seriesCount
				Dim delay As Single = Me.properties.bulletDelay.GetFloatAt(1F - CSng(groups) / (CSng(Me.properties.seriesCount) - 1F))
				While shots < Me.properties.bulletCount
					shots += 1
					MyBase.animator.SetTrigger("Shoot")
					Me.didShoot = False
					Me.projectileParryFlag = shots = Me.properties.bulletCount
					While Not Me.didShoot
						Yield Nothing
					End While
					Yield CupheadTime.WaitForSeconds(Me, delay)
				End While
				groups += 1
				shots = 0
				If groups <> Me.properties.seriesCount Then
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.seriesDelay)
					Yield CupheadTime.WaitForSeconds(Me, 0.6F)
				End If
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.idleTime)
		End While
		Return
	End Function

	' Token: 0x0600318B RID: 12683 RVA: 0x001CEAA7 File Offset: 0x001CCEA7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectilePrefab = Nothing
		Me.spitEffect = Nothing
	End Sub

	' Token: 0x040039F7 RID: 14839
	Private Const START_SHOOTING_TIME As Single = 0.6F

	' Token: 0x040039F9 RID: 14841
	<SerializeField()>
	Private gunRoot As Transform

	' Token: 0x040039FA RID: 14842
	<SerializeField()>
	Private projectilePrefab As VeggiesLevelSpit

	' Token: 0x040039FB RID: 14843
	<SerializeField()>
	Private spitEffect As Effect

	' Token: 0x040039FC RID: 14844
	Private properties As LevelProperties.Veggies.Potato

	' Token: 0x040039FD RID: 14845
	Private hp As Single

	' Token: 0x040039FE RID: 14846
	Private damageDealer As DamageDealer

	' Token: 0x040039FF RID: 14847
	Private didShoot As Boolean = True

	' Token: 0x04003A00 RID: 14848
	Private projectileParryFlag As Boolean

	' Token: 0x02000857 RID: 2135
	Public Enum State
		' Token: 0x04003A03 RID: 14851
		Incomplete
		' Token: 0x04003A04 RID: 14852
		Complete
	End Enum

	' Token: 0x02000858 RID: 2136
	' (Invoke) Token: 0x0600318D RID: 12685
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
