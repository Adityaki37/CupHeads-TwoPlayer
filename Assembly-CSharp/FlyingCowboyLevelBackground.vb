Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000648 RID: 1608
Public Class FlyingCowboyLevelBackground
	Inherits AbstractPausableComponent

	' Token: 0x06002101 RID: 8449 RVA: 0x00130D40 File Offset: 0x0012F140
	Private Sub Start()
		Me.sunsetInitialY = Me.skyLoopTransform.position.y
	End Sub

	' Token: 0x06002102 RID: 8450 RVA: 0x00130D68 File Offset: 0x0012F168
	Private Sub Update()
		If Me.sunsetTimeElapsed < Me.sunsetDuration Then
			Me.sunsetTimeElapsed += CupheadTime.Delta
			Dim position As Vector3 = Me.skyLoopTransform.position
			position.y = Mathf.Lerp(Me.sunsetInitialY, Me.sunsetTargetY, Me.sunsetTimeElapsed / Me.sunsetDuration)
			Me.skyLoopTransform.position = position
		End If
	End Sub

	' Token: 0x06002103 RID: 8451 RVA: 0x00130DDC File Offset: 0x0012F1DC
	Public Sub BeginTransition()
		If Me.transitionStarted Then
			Return
		End If
		Me.transitionStarted = True
		Me.initialScrollingMidLayer.looping = False
		Dim spriteRenderer As SpriteRenderer = Nothing
		For Each spriteRenderer2 As SpriteRenderer In Me.initialScrollingMidLayer.copyRenderers
			If spriteRenderer Is Nothing OrElse spriteRenderer.transform.position.x < spriteRenderer2.transform.position.x Then
				spriteRenderer = spriteRenderer2
			End If
		Next
		Dim x As Single = spriteRenderer.sprite.bounds.size.x
		Dim x2 As Single = Me.transitionBackground.GetComponent(Of SpriteRenderer)().bounds.size.x
		Dim position As Vector3 = Me.transitionBackground.transform.position
		position.x = spriteRenderer.transform.position.x + x * 0.5F + x2 * 0.5F - Me.initialScrollingMidLayer.speed * CupheadTime.Delta
		Me.transitionBackground.transform.position = position
		Me.transitionBackground.SetActive(True)
		Dim vector As Vector3 = Me.phase3Background.transform.position
		vector.x = position.x + x2 - Me.phase3Scrolling.offset
		Me.phase3Background.transform.position = vector
		Me.phase3Background.SetActive(True)
		MyBase.StartCoroutine(Me.transitionScroll_cr(Me.initialScrollingMidLayer.speed, x))
		vector = Me.phase3Foreground.transform.position
		vector.x = Me.transitionBackground.transform.position.x
		Me.phase3Foreground.transform.position = vector
		Me.phase3Foreground.SetActive(True)
		MyBase.StartCoroutine(Me.foregroundTransitionScroll_cr(Me.initialScrollingMidLayer.speed))
	End Sub

	' Token: 0x06002104 RID: 8452 RVA: 0x00131018 File Offset: 0x0012F418
	Private Iterator Function transitionScroll_cr(speed As Single, size As Single) As IEnumerator
		Dim displacement As Single
		Dim totalDisplacement As Single = 0F
		While totalDisplacement < 3F * size
			Yield Nothing
			displacement = speed * CupheadTime.Delta
			Dim position As Vector3 = Me.transitionBackground.transform.position
			position.x -= displacement
			Me.transitionBackground.transform.position = position
			If Not Me.phase3Scrolling.enabled Then
				position = Me.phase3Background.transform.position
				position.x -= displacement
				Me.phase3Background.transform.position = position
				If Me.phase3Background.transform.position.x < 0F Then
					Me.phase3Scrolling.enabled = True
					For Each scrollingSpriteSpawner As ScrollingSpriteSpawner In Me.phase3MidSpawners
						scrollingSpriteSpawner.StartLoop(True)
					Next
				End If
			End If
			totalDisplacement += displacement
		End While
		Me.initialScrollingMidLayer.gameObject.SetActive(False)
		Me.transitionBackground.SetActive(False)
		Return
	End Function

	' Token: 0x06002105 RID: 8453 RVA: 0x00131044 File Offset: 0x0012F444
	Private Iterator Function foregroundTransitionScroll_cr(speed As Single) As IEnumerator
		Dim transform As Transform = Me.phase3Foreground.transform
		transform.AddPosition(400F, 0F, 0F)
		Dim startTransform As Transform = Me.phase3ForegroundStart.transform
		Dim initialPropsDisabled As Boolean = False
		While transform.position.x > 0F
			Yield Nothing
			Dim positionX As Single = startTransform.position.x
			If Not initialPropsDisabled AndAlso positionX <= 2480F Then
				initialPropsDisabled = True
				For Each scrollingSpriteSpawner As ScrollingSpriteSpawner In Me.initialFGSpawners
					scrollingSpriteSpawner.StopAllCoroutines()
					scrollingSpriteSpawner.enabled = False
				Next
			End If
			If speed <> Me.phase3ForegroundScrolling.speed AndAlso positionX <= 2080F Then
				speed = Me.phase3ForegroundScrolling.speed
			End If
			Dim position As Vector3 = transform.position
			position.x -= speed * CupheadTime.Delta
			transform.position = position
		End While
		Me.phase3ForegroundScrolling.enabled = True
		For Each scrollingSpriteSpawner2 As ScrollingSpriteSpawner In Me.phase3FGSpawners
			scrollingSpriteSpawner2.StartLoop(True)
		Next
		Return
	End Function

	' Token: 0x0400299A RID: 10650
	<SerializeField()>
	Private sunsetDuration As Single

	' Token: 0x0400299B RID: 10651
	<SerializeField()>
	Private sunsetTargetY As Single

	' Token: 0x0400299C RID: 10652
	<SerializeField()>
	Private skyLoopTransform As Transform

	' Token: 0x0400299D RID: 10653
	<SerializeField()>
	Private initialScrollingMidLayer As FlyingCowboyLevelOverlayScrollingSprite

	' Token: 0x0400299E RID: 10654
	<SerializeField()>
	Private initialFGSpawners As ScrollingSpriteSpawner()

	' Token: 0x0400299F RID: 10655
	<SerializeField()>
	Private transitionBackground As GameObject

	' Token: 0x040029A0 RID: 10656
	<SerializeField()>
	Private phase3Background As GameObject

	' Token: 0x040029A1 RID: 10657
	<SerializeField()>
	Private phase3Scrolling As ScrollingSprite

	' Token: 0x040029A2 RID: 10658
	<SerializeField()>
	Private phase3MidSpawners As ScrollingSpriteSpawner()

	' Token: 0x040029A3 RID: 10659
	<SerializeField()>
	Private phase3Foreground As GameObject

	' Token: 0x040029A4 RID: 10660
	<SerializeField()>
	Private phase3ForegroundStart As GameObject

	' Token: 0x040029A5 RID: 10661
	<SerializeField()>
	Private phase3ForegroundScrolling As ScrollingSprite

	' Token: 0x040029A6 RID: 10662
	<SerializeField()>
	Private phase3FGSpawners As ScrollingSpriteSpawner()

	' Token: 0x040029A7 RID: 10663
	Private sunsetTimeElapsed As Single

	' Token: 0x040029A8 RID: 10664
	Private sunsetInitialY As Single

	' Token: 0x040029A9 RID: 10665
	Private transitionStarted As Boolean
End Class
