Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020004B0 RID: 1200
Public Class AirplaneLevelBackgroundHandler
	Inherits MonoBehaviour

	' Token: 0x06001396 RID: 5014 RVA: 0x000AC382 File Offset: 0x000AA782
	Private Sub Start()
		Me.hillsFrameIndex = Me.hillsSprites.Length - 1
		MyBase.StartCoroutine(Me.main_loop_cr())
		MyBase.StartCoroutine(Me.cloud_loop_cr())
	End Sub

	' Token: 0x06001397 RID: 5015 RVA: 0x000AC3B0 File Offset: 0x000AA7B0
	Private Iterator Function cloud_loop_cr() As IEnumerator
		Dim useAlternate As Boolean() = New Boolean(7) {}
		Dim lastCloud As Integer() = New Integer() { -1, -1, -1 }
		For i As Integer = 0 To 4 - 1
			Dim num As Integer = Global.UnityEngine.Random.Range(0, 8)
			If num <> 3 AndAlso num <> lastCloud(0) AndAlso num <> lastCloud(1) AndAlso num <> lastCloud(2) Then
				Me.cloudRenderers(i).flipX = num >= 4
				Me.cloudAnimators(i).Play((num Mod 4 * 2 + If((Not useAlternate(num)), 1, 0)).ToString(), 0, Global.UnityEngine.Random.Range(0F, 1F))
				useAlternate(num) = Not useAlternate(num)
				lastCloud(2) = lastCloud(1)
				lastCloud(1) = lastCloud(0)
				lastCloud(0) = num
			End If
		Next
		While True
			Dim delay As Integer = Global.UnityEngine.Random.Range(Me.CLOUD_DELAY_FRAMES_MIN, Me.CLOUD_DELAY_FRAMES_MAX)
			Dim t As Integer = 0
			While t < delay
				If Not CupheadTime.IsPaused() Then
					t += 1
				End If
				Yield Nothing
			End While
			Dim myRenderer As Integer = -1
			For j As Integer = 0 To Me.cloudRenderers.Length - 1
				If Me.cloudRenderers(j).sprite Is Nothing Then
					myRenderer = j
					Exit For
				End If
			Next
			If myRenderer = -1 Then
				Yield Nothing
			Else
				Dim num2 As Integer = Global.UnityEngine.Random.Range(0, 8)
				If num2 = 3 AndAlso CSng(Global.UnityEngine.Random.Range(0, 1)) < 0.75F Then
					num2 += Global.UnityEngine.Random.Range(1, 3) * If((Not MathUtils.RandomBool()), 1, (-1))
				End If
				If num2 <> lastCloud(0) AndAlso num2 <> lastCloud(1) AndAlso num2 <> lastCloud(2) Then
					Me.cloudRenderers(myRenderer).flipX = num2 >= 4
					If num2 = 3 Then
						Me.cloudRenderers(myRenderer).flipX = MathUtils.RandomBool()
					End If
					Me.cloudAnimators(myRenderer).Play((num2 Mod 4 * 2 + If((Not useAlternate(num2)), 1, 0)).ToString(), 0, 0F)
					useAlternate(num2) = Not useAlternate(num2)
					lastCloud(2) = lastCloud(1)
					lastCloud(1) = lastCloud(0)
					lastCloud(0) = num2
				End If
			End If
		End While
		Return
	End Function

	' Token: 0x06001398 RID: 5016 RVA: 0x000AC3CC File Offset: 0x000AA7CC
	Private Iterator Function play_object_cr(objectNum As Integer, myIndex As Integer) As IEnumerator
		Me.groundControllerCoroutineCurrentObject.Add(objectNum)
		While True
			While Me.groundControllerCoroutineCurrentObject(myIndex) = -1
				Yield Nothing
			End While
			objectNum = Me.groundControllerCoroutineCurrentObject(myIndex)
			While Me.hillsFrameIndex < Me.objects(objectNum).startFrame
				Yield Nothing
			End While
			Dim myRenderer As Integer = -1
			For i As Integer = 0 To Me.spriteRenderers.Length - 1
				If Me.spriteRenderers(i).sprite Is Nothing Then
					myRenderer = i
					Exit For
				End If
			Next
			If myRenderer <> -1 Then
				Dim frameCounter As Integer = 0
				Dim curHillsFrameIndex As Integer = Me.hillsFrameIndex
				While frameCounter < Me.objects(objectNum).duration
					Me.spriteRenderers(myRenderer).sprite = Me.objectSprites(Me.objects(objectNum).spriteIndex + frameCounter)
					Me.spriteRenderers(myRenderer).sortingOrder = -100 - frameCounter * 2 + Me.objects(objectNum).layerOffset
					While curHillsFrameIndex = Me.hillsFrameIndex
						Yield Nothing
					End While
					curHillsFrameIndex = Me.hillsFrameIndex
					frameCounter += 1
				End While
				Me.spriteRenderers(myRenderer).sprite = Nothing
			End If
			Me.groundControllerCoroutineCurrentObject(myIndex) = -1
		End While
		Return
	End Function

	' Token: 0x06001399 RID: 5017 RVA: 0x000AC3F8 File Offset: 0x000AA7F8
	Private Iterator Function main_loop_cr() As IEnumerator
		Dim startObject As Boolean() = New Boolean(Me.objects.Length - 1) {}
		While True
			Me.hillsFrameIndex = (Me.hillsFrameIndex + 1) Mod Me.hillsSprites.Length
			If Me.hillsFrameIndex = 0 Then
				Me.densityWavePosition += Me.densityWaveRate
				For i As Integer = 0 To Me.objects.Length - 1
					startObject(i) = Global.UnityEngine.Random.Range(0F, 1F) < 0.4F + Mathf.Sin(Me.densityWavePosition) * 0.2F
				Next
				If startObject(3) AndAlso startObject(5) Then
					startObject(If((Not MathUtils.RandomBool()), 5, 3)) = False
				End If
				For j As Integer = 0 To Me.objects.Length - 1
					If startObject(j) Then
						Dim num As Integer = -1
						For k As Integer = 0 To Me.groundControllerCoroutine.Count - 1
							If Me.groundControllerCoroutineCurrentObject(k) = -1 Then
								num = k
								Exit For
							End If
						Next
						If num > -1 Then
							Me.groundControllerCoroutineCurrentObject(num) = j
						Else
							Me.groundControllerCoroutine.Add(MyBase.StartCoroutine(Me.play_object_cr(j, Me.groundControllerCoroutine.Count)))
						End If
					End If
				Next
			End If
			If Me.prepopulateCounter >= 48 Then
				Yield New WaitForEndOfFrame()
			End If
			Me.hillsRenderer.sprite = Me.hillsSprites(Me.hillsFrameIndex)
			Me.bgFillSprite.color = Me.bgColor(Me.hillsFrameIndex)
			If Me.prepopulateCounter >= 48 Then
				Yield CupheadTime.WaitForSeconds(Me, 1F / Me.frameRate)
			Else
				Yield Nothing
			End If
			Me.prepopulateCounter += 1
		End While
		Return
	End Function

	' Token: 0x0600139A RID: 5018 RVA: 0x000AC414 File Offset: 0x000AA814
	Private Sub Update()
		Me.distantHillsTimer += CupheadTime.Delta
		Dim num As Single = Me.distantHillsTimer Mod (Me.distantHillsLoopTime * CSng(Me.distantHillsRenderers.Length)) / (Me.distantHillsLoopTime * CSng(Me.distantHillsRenderers.Length))
		For i As Integer = 0 To Me.distantHillsRenderers.Length - 1
			Dim num2 As Single = EaseUtils.EaseOutCubic(Me.distantHillsMaxScale, Me.distantHillsMinScale, (num + CSng(i) * (1F / CSng(Me.distantHillsRenderers.Length))) Mod 1F)
			Me.distantHillsRenderers(i).transform.localScale = New Vector3(num2, num2)
			Me.distantHillsRenderers(i).sortingOrder = -490 - CInt(((num * CSng(Me.distantHillsRenderers.Length) + CSng(i)) Mod CSng(Me.distantHillsRenderers.Length)))
			Me.distantHillsRenderers(i).color = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(Me.distantHillsFadeStartScale, Me.distantHillsMinScale, num2))
		Next
	End Sub

	' Token: 0x04001CA1 RID: 7329
	Private CLOUD_DELAY_FRAMES_MIN As Integer = 5

	' Token: 0x04001CA2 RID: 7330
	Private CLOUD_DELAY_FRAMES_MAX As Integer = 10

	' Token: 0x04001CA3 RID: 7331
	<SerializeField()>
	Private frameRate As Single = 30F

	' Token: 0x04001CA4 RID: 7332
	<SerializeField()>
	Private bgColor As Color()

	' Token: 0x04001CA5 RID: 7333
	<SerializeField()>
	Private bgFillSprite As SpriteRenderer

	' Token: 0x04001CA6 RID: 7334
	<SerializeField()>
	Private hillsSprites As Sprite()

	' Token: 0x04001CA7 RID: 7335
	<SerializeField()>
	Private hillsRenderer As SpriteRenderer

	' Token: 0x04001CA8 RID: 7336
	<SerializeField()>
	Private objects As AirplaneLevelBackgroundHandler.bgObject()

	' Token: 0x04001CA9 RID: 7337
	<SerializeField()>
	Private spriteRenderers As SpriteRenderer()

	' Token: 0x04001CAA RID: 7338
	<SerializeField()>
	Private objectSprites As Sprite()

	' Token: 0x04001CAB RID: 7339
	<SerializeField()>
	Private cloudRenderers As SpriteRenderer()

	' Token: 0x04001CAC RID: 7340
	<SerializeField()>
	Private cloudAnimators As Animator()

	' Token: 0x04001CAD RID: 7341
	<SerializeField()>
	Private distantHillsRenderers As SpriteRenderer()

	' Token: 0x04001CAE RID: 7342
	Private distantHillsTimer As Single

	' Token: 0x04001CAF RID: 7343
	<SerializeField()>
	Private distantHillsLoopTime As Single = 40F

	' Token: 0x04001CB0 RID: 7344
	<SerializeField()>
	Private distantHillsMaxScale As Single = 3F

	' Token: 0x04001CB1 RID: 7345
	<SerializeField()>
	Private distantHillsMinScale As Single = 0.04F

	' Token: 0x04001CB2 RID: 7346
	<SerializeField()>
	Private distantHillsFadeStartScale As Single = 0.2F

	' Token: 0x04001CB3 RID: 7347
	Private groundControllerCoroutine As List(Of Coroutine) = New List(Of Coroutine)()

	' Token: 0x04001CB4 RID: 7348
	Private groundControllerCoroutineCurrentObject As List(Of Integer) = New List(Of Integer)()

	' Token: 0x04001CB5 RID: 7349
	Private hillsFrameIndex As Integer

	' Token: 0x04001CB6 RID: 7350
	Private prepopulateCounter As Integer

	' Token: 0x04001CB7 RID: 7351
	Private densityWavePosition As Single

	' Token: 0x04001CB8 RID: 7352
	Private densityWaveRate As Single = 0.5F

	' Token: 0x020004B1 RID: 1201
	<Serializable()>
	Private Structure bgObject
		' Token: 0x04001CB9 RID: 7353
		Public nameForSanity As String

		' Token: 0x04001CBA RID: 7354
		Public startFrame As Integer

		' Token: 0x04001CBB RID: 7355
		Public duration As Integer

		' Token: 0x04001CBC RID: 7356
		Public spriteIndex As Integer

		' Token: 0x04001CBD RID: 7357
		Public layerOffset As Integer
	End Structure
End Class
