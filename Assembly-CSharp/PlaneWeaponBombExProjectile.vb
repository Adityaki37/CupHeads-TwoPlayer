Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000AB6 RID: 2742
Public Class PlaneWeaponBombExProjectile
	Inherits AbstractProjectile

	' Token: 0x170005C3 RID: 1475
	' (get) Token: 0x060041DC RID: 16860 RVA: 0x002398E8 File Offset: 0x00237CE8
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 100F
		End Get
	End Property

	' Token: 0x060041DD RID: 16861 RVA: 0x002398F0 File Offset: 0x00237CF0
	Public Sub Init()
		Me.Cuphead.enabled = (Me.PlayerId = PlayerId.PlayerOne AndAlso Not PlayerManager.player1IsMugman) OrElse (Me.PlayerId = PlayerId.PlayerTwo AndAlso PlayerManager.player1IsMugman)
		Me.Mugman.enabled = (Me.PlayerId = PlayerId.PlayerOne AndAlso PlayerManager.player1IsMugman) OrElse (Me.PlayerId = PlayerId.PlayerTwo AndAlso Not PlayerManager.player1IsMugman)
		MyBase.StartCoroutine(Me.trail_cr())
	End Sub

	' Token: 0x060041DE RID: 16862 RVA: 0x0023997B File Offset: 0x00237D7B
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If hit.tag = "Parry" Then
			Return
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x060041DF RID: 16863 RVA: 0x0023999B File Offset: 0x00237D9B
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		Me.DealDamage(hit)
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x060041E0 RID: 16864 RVA: 0x002399AC File Offset: 0x00237DAC
	Private Sub DealDamage(hit As GameObject)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060041E1 RID: 16865 RVA: 0x002399BB File Offset: 0x00237DBB
	Protected Overrides Sub Die()
		Me.move = False
		MyBase.transform.rotation = Quaternion.Euler(0F, 0F, Global.UnityEngine.Random.Range(0F, 360F))
		MyBase.Die()
	End Sub

	' Token: 0x060041E2 RID: 16866 RVA: 0x002399F4 File Offset: 0x00237DF4
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.move Then
			Return
		End If
		Me.t += CupheadTime.FixedDelta
		If Me.target IsNot Nothing AndAlso Me.target.gameObject.activeInHierarchy AndAlso Me.target.isActiveAndEnabled AndAlso Me.t < WeaponProperties.LevelWeaponHoming.Basic.maxHomingTime Then
			Dim num As Single
			num = MathUtils.DirectionToAngle(Me.target.bounds.center - MyBase.transform.position)
			While num > Me.rotation + 180F
				num -= 360F
			End While
			While num < Me.rotation - 180F
				num += 360F
			End While
			Dim num2 As Single = Me.rotationSpeed.min
			If Me.t > Me.timeBeforeEaseRotationSpeed + Me.rotationSpeedEaseTime Then
				num2 = Me.rotationSpeed.max
			ElseIf Me.t > Me.timeBeforeEaseRotationSpeed Then
				num2 = Me.rotationSpeed.GetFloatAt((Me.t - Me.timeBeforeEaseRotationSpeed) / Me.rotationSpeedEaseTime)
			End If
			If Mathf.Abs(num - Me.rotation) < num2 * CupheadTime.FixedDelta Then
				Me.rotation = num
			ElseIf num > Me.rotation Then
				Me.rotation += num2 * CupheadTime.FixedDelta
			Else
				Me.rotation -= num2 * CupheadTime.FixedDelta
			End If
		End If
		Dim vector As Vector3 = MathUtils.AngleToDirection(Me.rotation)
		MyBase.transform.position += vector * Me.speed * CupheadTime.FixedDelta
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(Me.rotation + Me.spriteRotation))
		If Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(Me.destroyPadding, Me.destroyPadding)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060041E3 RID: 16867 RVA: 0x00239C44 File Offset: 0x00238044
	Public Sub FindTarget()
		Dim num As Single = Single.MaxValue
		Dim collider2D As Collider2D = Nothing
		Dim vector As Vector2 = MyBase.transform.position + Me.speed * (Me.timeBeforeEaseRotationSpeed + Me.rotationSpeedEaseTime * 0.75F) * MathUtils.AngleToDirection(Me.rotation)
		For Each damageReceiver As DamageReceiver In Global.UnityEngine.[Object].FindObjectsOfType(Of DamageReceiver)()
			If damageReceiver.gameObject.activeInHierarchy AndAlso damageReceiver.type = DamageReceiver.Type.Enemy Then
				For Each collider2D2 As Collider2D In damageReceiver.GetComponents(Of Collider2D)()
					If collider2D2.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2F) Then
						Dim sqrMagnitude As Single = (vector - collider2D2.bounds.center).sqrMagnitude
						If sqrMagnitude < num Then
							num = sqrMagnitude
							collider2D = collider2D2
						End If
					End If
				Next
				For Each damageReceiverChild As DamageReceiverChild In damageReceiver.GetComponentsInChildren(Of DamageReceiverChild)()
					For Each collider2D3 As Collider2D In damageReceiverChild.GetComponents(Of Collider2D)()
						If collider2D3.isActiveAndEnabled AndAlso CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2F) Then
							Dim sqrMagnitude2 As Single = (vector - collider2D3.bounds.center).sqrMagnitude
							If sqrMagnitude2 < num Then
								num = sqrMagnitude2
								collider2D = collider2D3
							End If
						End If
					Next
				Next
			End If
		Next
		Me.target = collider2D
	End Sub

	' Token: 0x060041E4 RID: 16868 RVA: 0x00239E68 File Offset: 0x00238268
	Private Iterator Function trail_cr() As IEnumerator
		While Not MyBase.dead
			Yield CupheadTime.WaitForSeconds(Me, Me.trailDelay)
			If MyBase.dead Then
				Return
			End If
			Me.trailFxPrefab.Create(Me.trailFxRoot.position + MathUtils.RandomPointInUnitCircle() * Me.trailFxMaxOffset)
		End While
		Return
	End Function

	' Token: 0x060041E5 RID: 16869 RVA: 0x00239E83 File Offset: 0x00238283
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x0400483C RID: 18492
	<SerializeField()>
	Private spriteRotation As Single

	' Token: 0x0400483D RID: 18493
	<SerializeField()>
	Private trailFxPrefab As Effect

	' Token: 0x0400483E RID: 18494
	<SerializeField()>
	Private trailFxRoot As Transform

	' Token: 0x0400483F RID: 18495
	<SerializeField()>
	Private trailFxMaxOffset As Single

	' Token: 0x04004840 RID: 18496
	<SerializeField()>
	Private trailDelay As Single

	' Token: 0x04004841 RID: 18497
	<SerializeField()>
	Private destroyPadding As Single

	' Token: 0x04004842 RID: 18498
	<SerializeField()>
	Private Cuphead As SpriteRenderer

	' Token: 0x04004843 RID: 18499
	<SerializeField()>
	Private Mugman As SpriteRenderer

	' Token: 0x04004844 RID: 18500
	Public speed As Single

	' Token: 0x04004845 RID: 18501
	Public rotationSpeed As MinMax

	' Token: 0x04004846 RID: 18502
	Public timeBeforeEaseRotationSpeed As Single

	' Token: 0x04004847 RID: 18503
	Public rotationSpeedEaseTime As Single

	' Token: 0x04004848 RID: 18504
	Public rotation As Single

	' Token: 0x04004849 RID: 18505
	Private velocity As Vector2

	' Token: 0x0400484A RID: 18506
	Private t As Single

	' Token: 0x0400484B RID: 18507
	Private move As Boolean = True

	' Token: 0x0400484C RID: 18508
	Private target As Collider2D

	' Token: 0x0400484D RID: 18509
	Private player As AbstractPlayerController
End Class
