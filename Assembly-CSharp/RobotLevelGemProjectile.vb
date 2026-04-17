Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200077C RID: 1916
Public Class RobotLevelGemProjectile
	Inherits AbstractProjectile

	' Token: 0x170003E7 RID: 999
	' (get) Token: 0x060029FC RID: 10748 RVA: 0x00188F83 File Offset: 0x00187383
	' (set) Token: 0x060029FD RID: 10749 RVA: 0x00188F8B File Offset: 0x0018738B
	Public Property Speed As Single

	' Token: 0x170003E8 RID: 1000
	' (get) Token: 0x060029FE RID: 10750 RVA: 0x00188F94 File Offset: 0x00187394
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return Me.lifeTime
		End Get
	End Property

	' Token: 0x060029FF RID: 10751 RVA: 0x00188F9C File Offset: 0x0018739C
	Public Overridable Function Init(speed As MinMax, acceleration As Single, waveLength As Single, waveSpeedMultiplier As Single, lifeTime As Single, isBlue As Boolean, isParryable As Boolean) As AbstractProjectile
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Dim num As Single = Global.UnityEngine.Random.Range(0.92F, 1.08F)
		Me.minSpeed = speed.min * num
		Me.maxSpeed = speed.max * num
		Me.Speed = Me.minSpeed
		Me.acceleration = acceleration
		Me.waveLength = waveLength
		Me.waveSpeedMultiplier = waveSpeedMultiplier
		Me.lifeTime = lifeTime
		MyBase.animator.SetFloat("Gem", CSng(If((Not isBlue), 0, 1)))
		Me.time = 0F
		Me.originalPosition = MyBase.transform.position
		Me.SetParryable(isParryable)
		If isParryable Then
			MyBase.animator.Play("GemParry", 0, Global.UnityEngine.Random.value)
		Else
			MyBase.animator.Play("Gem", 0, Global.UnityEngine.Random.value)
		End If
		MyBase.StartCoroutine(Me.speed_cr())
		MyBase.StartCoroutine(Me.fadeIn_cr())
		Return Me
	End Function

	' Token: 0x06002A00 RID: 10752 RVA: 0x001890A4 File Offset: 0x001874A4
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.originalPosition += -MyBase.transform.right * Me.Speed * CupheadTime.Delta
		MyBase.transform.position = Me.originalPosition + Mathf.Sin(Me.time * Me.waveSpeedMultiplier) * Me.waveLength * MyBase.transform.up
		Me.time += CupheadTime.Delta
	End Sub

	' Token: 0x06002A01 RID: 10753 RVA: 0x00189148 File Offset: 0x00187548
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002A02 RID: 10754 RVA: 0x00189166 File Offset: 0x00187566
	Private Sub SetCollider(c As Boolean)
		MyBase.GetComponent(Of CircleCollider2D)().enabled = c
	End Sub

	' Token: 0x06002A03 RID: 10755 RVA: 0x00189174 File Offset: 0x00187574
	Private Iterator Function effect_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.3F))
		While True
			Me.effectPrefab.Create(Me.effectRoot.position)
			Yield CupheadTime.WaitForSeconds(Me, 0.3F)
		End While
		Return
	End Function

	' Token: 0x06002A04 RID: 10756 RVA: 0x00189190 File Offset: 0x00187590
	Private Iterator Function speed_cr() As IEnumerator
		Me.Speed = Me.minSpeed
		While Me.Speed < Me.maxSpeed
			Me.Speed += Me.acceleration
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002A05 RID: 10757 RVA: 0x001891AC File Offset: 0x001875AC
	Private Iterator Function fadeIn_cr() As IEnumerator
		Dim sprite As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		While sprite.color.a < 1F
			Dim c As Color = sprite.color
			c.a += 1F * CupheadTime.Delta
			sprite.color = c
			Yield Nothing
		End While
		Dim color As Color = sprite.color
		color.a = 1F
		sprite.color = color
		Return
	End Function

	' Token: 0x06002A06 RID: 10758 RVA: 0x001891C7 File Offset: 0x001875C7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.effectPrefab = Nothing
	End Sub

	' Token: 0x06002A07 RID: 10759 RVA: 0x001891D6 File Offset: 0x001875D6
	Public Overrides Sub OnParryDie()
		Me.Recycle()
	End Sub

	' Token: 0x06002A08 RID: 10760 RVA: 0x001891DE File Offset: 0x001875DE
	Protected Overrides Sub OnDieDistance()
		Me.Recycle()
	End Sub

	' Token: 0x06002A09 RID: 10761 RVA: 0x001891E6 File Offset: 0x001875E6
	Protected Overrides Sub OnDieLifetime()
		Me.Recycle()
	End Sub

	' Token: 0x06002A0A RID: 10762 RVA: 0x001891EE File Offset: 0x001875EE
	Protected Overrides Sub OnDieAnimationComplete()
		Me.Recycle()
	End Sub

	' Token: 0x040032DE RID: 13022
	Private Const GemParameterName As String = "Gem"

	' Token: 0x040032DF RID: 13023
	Private Const GemParryParameterName As String = "GemParry"

	' Token: 0x040032E0 RID: 13024
	Private Const SpeedVariation As Single = 0.08F

	' Token: 0x040032E1 RID: 13025
	Private Const FadeTime As Single = 0.3F

	' Token: 0x040032E2 RID: 13026
	Private Const FadeRate As Single = 0.3F

	' Token: 0x040032E4 RID: 13028
	<SerializeField()>
	Private effectPrefab As Effect

	' Token: 0x040032E5 RID: 13029
	<SerializeField()>
	Private effectRoot As Transform

	' Token: 0x040032E6 RID: 13030
	Private originalPosition As Vector3

	' Token: 0x040032E7 RID: 13031
	Private originalScale As Vector3

	' Token: 0x040032E8 RID: 13032
	Private minSpeed As Single

	' Token: 0x040032E9 RID: 13033
	Private maxSpeed As Single

	' Token: 0x040032EA RID: 13034
	Private acceleration As Single

	' Token: 0x040032EB RID: 13035
	Private waveLength As Single

	' Token: 0x040032EC RID: 13036
	Private waveSpeedMultiplier As Single

	' Token: 0x040032ED RID: 13037
	Private time As Single

	' Token: 0x040032EE RID: 13038
	Private lifeTime As Single
End Class
