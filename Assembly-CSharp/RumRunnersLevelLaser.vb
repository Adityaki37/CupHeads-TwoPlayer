Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000790 RID: 1936
Public Class RumRunnersLevelLaser
	Inherits AbstractCollidableObject

	' Token: 0x06002ADE RID: 10974 RVA: 0x00190028 File Offset: 0x0018E428
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		For Each collisionChild As CollisionChild In Me.childColliders
			AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Next
	End Sub

	' Token: 0x06002ADF RID: 10975 RVA: 0x00190078 File Offset: 0x0018E478
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002AE0 RID: 10976 RVA: 0x00190090 File Offset: 0x0018E490
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Dim num As Single = Me.damageDealer.DealDamage(hit)
		If num > 0F Then
			Me.SFX_RUMRUN_Grammobeam_DamagePlayer()
		End If
	End Sub

	' Token: 0x06002AE1 RID: 10977 RVA: 0x001900C4 File Offset: 0x0018E4C4
	Private Iterator Function moveMask_cr(laserMask As GameObject, startX As Single, endX As Single, duration As Single, destroyMask As Boolean) As IEnumerator
		Dim elapsedTime As Single = 0F
		While elapsedTime < duration
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			laserMask.transform.SetLocalPosition(New Single?(EaseUtils.Linear(startX, endX, elapsedTime / duration)), Nothing, Nothing)
		End While
		If destroyMask Then
			Global.UnityEngine.[Object].Destroy(laserMask)
		End If
		Return
	End Function

	' Token: 0x06002AE2 RID: 10978 RVA: 0x001900FD File Offset: 0x0018E4FD
	Public Sub Begin()
		MyBase.StartCoroutine(Me.begin_cr(Me.mainRenderers, 1F))
	End Sub

	' Token: 0x06002AE3 RID: 10979 RVA: 0x00190118 File Offset: 0x0018E518
	Private Iterator Function begin_cr(renderers As SpriteRenderer(), Optional durationMultiplier As Single = 1F) As IEnumerator
		Dim DurationRange As MinMax = New MinMax(1.2F, 1.5F)
		For Each renderer As SpriteRenderer In renderers
			renderer.enabled = True
			Dim laserMask As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.laserMaskPrefab)
			laserMask.transform.parent = renderer.transform
			laserMask.transform.ResetLocalRotation()
			laserMask.transform.localPosition = New Vector3(-400F, 0F)
			laserMask.GetComponent(Of RumRunnersLevelLaserMask)().Setup(renderer.sortingLayerID, renderer.sortingOrder)
			MyBase.StartCoroutine(Me.moveMask_cr(laserMask, -400F, 800F, DurationRange.RandomFloat() * durationMultiplier, True))
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Next
		Return
	End Function

	' Token: 0x06002AE4 RID: 10980 RVA: 0x00190141 File Offset: 0x0018E541
	Public Sub [End]()
		MyBase.StartCoroutine(Me.end_cr())
	End Sub

	' Token: 0x06002AE5 RID: 10981 RVA: 0x00190150 File Offset: 0x0018E550
	Private Iterator Function end_cr() As IEnumerator
		Dim DurationRange As MinMax = New MinMax(1.2F, 1.5F)
		Dim coroutines As Coroutine() = New Coroutine(Me.mainRenderers.Length - 1) {}
		For i As Integer = 0 To Me.mainRenderers.Length - 1
			Dim renderer As SpriteRenderer = Me.mainRenderers(i)
			Dim laserMask As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.laserMaskPrefab)
			laserMask.transform.parent = renderer.transform
			laserMask.transform.ResetLocalRotation()
			laserMask.transform.localRotation = Quaternion.Euler(0F, 0F, 180F)
			laserMask.transform.localPosition = New Vector3(220F, 0F)
			laserMask.GetComponent(Of RumRunnersLevelLaserMask)().Setup(renderer.sortingLayerID, renderer.sortingOrder)
			coroutines(i) = MyBase.StartCoroutine(Me.moveMask_cr(laserMask, 220F, 1600F, DurationRange.RandomFloat(), False))
			MyBase.StartCoroutine(Me.endSparkles_cr(laserMask.transform))
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Next
		For Each coroutine As Coroutine In coroutines
			Yield coroutine
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002AE6 RID: 10982 RVA: 0x0019016C File Offset: 0x0018E56C
	Private Iterator Function endSparkles_cr(maskTransform As Transform) As IEnumerator
		Dim SpawnRandomizationRange As MinMax = New MinMax(-15F, 15F)
		Dim elapsedTime As Single = 0F
		While True
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			If elapsedTime >= 0.02F Then
				elapsedTime -= 0.02F
				For i As Integer = 0 To 1 - 1
					Me.sparklesEffect.Create(maskTransform.position + maskTransform.right * 280F + New Vector3(SpawnRandomizationRange.RandomFloat(), SpawnRandomizationRange.RandomFloat()))
				Next
			End If
		End While
		Return
	End Function

	' Token: 0x06002AE7 RID: 10983 RVA: 0x0019018E File Offset: 0x0018E58E
	Public Sub Warning()
		MyBase.StartCoroutine(Me.warning_cr())
	End Sub

	' Token: 0x06002AE8 RID: 10984 RVA: 0x001901A0 File Offset: 0x0018E5A0
	Private Iterator Function warning_cr() As IEnumerator
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.3F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			Dim alpha As Single = Mathf.Lerp(1F, 0F, elapsedTime / 0.3F)
			For i As Integer = 0 To Me.mainRenderers.Length - 1
				Dim color As Color = Me.mainRenderers(i).color
				color.a = alpha
				Me.mainRenderers(i).color = color
				color = Me.warningRenderers(i).color
				color.a = 1F - alpha
				Me.warningRenderers(i).color = color
			Next
		End While
		Return
	End Function

	' Token: 0x06002AE9 RID: 10985 RVA: 0x001901BC File Offset: 0x0018E5BC
	Public Sub CancelWarning()
		Me.StopAllCoroutines()
		For Each spriteRenderer As SpriteRenderer In Me.mainRenderers
			Dim color As Color = spriteRenderer.color
			color.a = 1F
			spriteRenderer.color = color
		Next
		For Each spriteRenderer2 As SpriteRenderer In Me.warningRenderers
			Dim color2 As Color = spriteRenderer2.color
			color2.a = 0F
			spriteRenderer2.color = color2
		Next
	End Sub

	' Token: 0x06002AEA RID: 10986 RVA: 0x00190250 File Offset: 0x0018E650
	Public Sub Attack()
		MyBase.animator.SetBool("On", True)
		For Each spriteRenderer As SpriteRenderer In Me.notesRenderers
			spriteRenderer.enabled = False
		Next
		MyBase.StartCoroutine(Me.begin_cr(Me.notesRenderers, 0.5F))
	End Sub

	' Token: 0x06002AEB RID: 10987 RVA: 0x001902AC File Offset: 0x0018E6AC
	Public Sub EndAttack()
		MyBase.animator.SetBool("On", False)
	End Sub

	' Token: 0x06002AEC RID: 10988 RVA: 0x001902C0 File Offset: 0x0018E6C0
	Private Sub animationEvent_WarningToOnStarted()
		For Each spriteRenderer As SpriteRenderer In Me.mainRenderers
			Dim color As Color = spriteRenderer.color
			color.a = 1F
			spriteRenderer.color = color
		Next
		For Each spriteRenderer2 As SpriteRenderer In Me.warningRenderers
			Dim color2 As Color = spriteRenderer2.color
			color2.a = 0F
			spriteRenderer2.color = color2
		Next
	End Sub

	' Token: 0x06002AED RID: 10989 RVA: 0x0019034C File Offset: 0x0018E74C
	Private Sub SFX_RUMRUN_Grammobeam_DamagePlayer()
		AudioManager.Play("sfx_dlc_rumrun_p2_grammobeam_damageplayer")
	End Sub

	' Token: 0x0400339D RID: 13213
	<SerializeField()>
	Private mainRenderers As SpriteRenderer()

	' Token: 0x0400339E RID: 13214
	<SerializeField()>
	Private warningRenderers As SpriteRenderer()

	' Token: 0x0400339F RID: 13215
	<SerializeField()>
	Private notesRenderers As SpriteRenderer()

	' Token: 0x040033A0 RID: 13216
	<SerializeField()>
	Private childColliders As CollisionChild()

	' Token: 0x040033A1 RID: 13217
	<SerializeField()>
	Private laserMaskPrefab As GameObject

	' Token: 0x040033A2 RID: 13218
	<SerializeField()>
	Private sparklesEffect As Effect

	' Token: 0x040033A3 RID: 13219
	Private damageDealer As DamageDealer
End Class
