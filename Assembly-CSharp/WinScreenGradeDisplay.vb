Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000B34 RID: 2868
Public Class WinScreenGradeDisplay
	Inherits AbstractMonoBehaviour

	' Token: 0x17000631 RID: 1585
	' (get) Token: 0x0600457C RID: 17788 RVA: 0x00249E1A File Offset: 0x0024821A
	' (set) Token: 0x0600457D RID: 17789 RVA: 0x00249E22 File Offset: 0x00248222
	Public Property Grade As LevelScoringData.Grade

	' Token: 0x17000632 RID: 1586
	' (get) Token: 0x0600457E RID: 17790 RVA: 0x00249E2B File Offset: 0x0024822B
	' (set) Token: 0x0600457F RID: 17791 RVA: 0x00249E33 File Offset: 0x00248233
	Public Property Difficulty As Level.Mode

	' Token: 0x17000633 RID: 1587
	' (get) Token: 0x06004580 RID: 17792 RVA: 0x00249E3C File Offset: 0x0024823C
	' (set) Token: 0x06004581 RID: 17793 RVA: 0x00249E44 File Offset: 0x00248244
	Public Property Celebration As Boolean

	' Token: 0x17000634 RID: 1588
	' (get) Token: 0x06004582 RID: 17794 RVA: 0x00249E4D File Offset: 0x0024824D
	' (set) Token: 0x06004583 RID: 17795 RVA: 0x00249E55 File Offset: 0x00248255
	Public Property FinishedGrading As Boolean

	' Token: 0x06004584 RID: 17796 RVA: 0x00249E5E File Offset: 0x0024825E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.input = New CupheadInput.AnyPlayerInput(False)
	End Sub

	' Token: 0x06004585 RID: 17797 RVA: 0x00249E74 File Offset: 0x00248274
	Private Sub Start()
		Me.Celebration = False
		If Level.PreviouslyWon Then
			Me.topGradeLabel.fontStyle = If((Localization.language <> Localization.Languages.Korean), Me.topGradeLabel.fontStyle, FontStyles.Bold)
			Me.topGradeValue.text = " " + Me.grades(CInt(Level.PreviousGrade))
		End If
	End Sub

	' Token: 0x06004586 RID: 17798 RVA: 0x00249EDA File Offset: 0x002482DA
	Public Sub Show()
		MyBase.StartCoroutine(Me.grade_tally_up_cr())
	End Sub

	' Token: 0x06004587 RID: 17799 RVA: 0x00249EEC File Offset: 0x002482EC
	Private Iterator Function grade_tally_up_cr() As IEnumerator
		Dim isTallying As Boolean = True
		Dim t As Single = 0F
		Dim counter As Integer = 0
		Me.text.text = Me.grades(Me.grades.Length - 1).Substring(0, 1) + " "
		While counter <= CInt(Me.Grade) AndAlso isTallying
			If counter >= CInt(Me.Grade) Then
				Exit While
			End If
			AudioManager.Play("win_score_tick")
			counter += 1
			Me.text.text = Me.grades(counter).Substring(0, 1) + " "
			While t < 0.02F
				If Me.input.GetButtonDown(CupheadButton.Accept) Then
					isTallying = False
					Exit While
				End If
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
		End While
		AudioManager.Play("win_grade_chalk")
		Me.circle.SetTrigger("Circle")
		Me.text.GetComponent(Of Animator)().SetTrigger("MakeBig")
		Me.text.text = Me.grades(CInt(Me.Grade))
		If counter = Me.grades.Length - 1 Then
			Me.text.color = ColorUtils.HexToColor("FCC93D")
		End If
		Dim PerfectGrade As LevelScoringData.Grade = If((Me.Difficulty <> Level.Mode.Hard), LevelScoringData.Grade.APlus, LevelScoringData.Grade.S)
		Dim english As Boolean = Localization.language = Localization.Languages.English
		If Not english Then
			Me.AlignBannerText()
		End If
		If Not Level.IsTowerOfPower Then
			If Me.Grade = PerfectGrade Then
				MyBase.StartCoroutine(Me.fade_text_cr())
				Yield CupheadTime.WaitForSeconds(Me, 0.16F)
				Me.gollyBanner.SetTrigger("OnBanner")
				Me.Celebration = True
				Me.LanguageUpdate(english)
				Me.gollyBannerEnglish.enabled = english
				Me.gollyBannerOther.enabled = Not english
				Yield Me.gollyBanner.WaitForAnimationToEnd(Me, "Golly", False, True)
			ElseIf Me.Grade > Level.PreviousGrade OrElse Not Level.PreviouslyWon Then
				MyBase.StartCoroutine(Me.fade_text_cr())
				Yield CupheadTime.WaitForSeconds(Me, 0.16F)
				Me.recordBanner.SetTrigger("OnBanner")
				Me.Celebration = True
				Me.LanguageUpdate(english)
				Me.recordBannerEnglish.enabled = english
				Me.recordBannerOther.enabled = Not english
				Yield Me.recordBanner.WaitForAnimationToEnd(Me, "Record", False, True)
			End If
		End If
		If Level.IsTowerOfPower AndAlso Me.Grade >= CType(TowerOfPowerLevelGameInfo.MIN_RANK_NEED_TO_GET_TOKEN, LevelScoringData.Grade) Then
			TowerOfPowerLevelGameInfo.AddToken()
		End If
		Me.FinishedGrading = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06004588 RID: 17800 RVA: 0x00249F08 File Offset: 0x00248308
	Private Sub AlignBannerText()
		For i As Integer = 0 To Me.normalBannerTexts.Length - 1
			Me.normalBannerTexts(i).GetComponent(Of TextMeshCurveAndJitter)().CurveScale = CSng(WinScreenGradeDisplay.NormalCurveValues(CInt(Localization.language)))
			Dim localPosition As Vector3 = Me.normalBannerTexts(i).transform.localPosition
			localPosition.y = CSng((-CSng(WinScreenGradeDisplay.NormalCurveOffsets(CInt(Localization.language)))))
			If i = Me.normalBannerTexts.Length - 1 Then
				localPosition.y += 2F
			End If
			Me.normalBannerTexts(i).transform.localPosition = localPosition
		Next
		For j As Integer = 0 To Me.topScoreBannerTexts.Length - 1
			Me.topScoreBannerTexts(j).GetComponent(Of TextMeshCurveAndJitter)().CurveScale = CSng(WinScreenGradeDisplay.GollyCurveValues(CInt(Localization.language)))
			Dim localPosition2 As Vector3 = Me.topScoreBannerTexts(j).transform.localPosition
			localPosition2.y = CSng((-CSng(WinScreenGradeDisplay.GollyCurveOffsets(CInt(Localization.language)))))
			If j = Me.topScoreBannerTexts.Length - 1 Then
				localPosition2.y -= 2F
			End If
			Me.topScoreBannerTexts(j).transform.localPosition = localPosition2
		Next
	End Sub

	' Token: 0x06004589 RID: 17801 RVA: 0x0024A040 File Offset: 0x00248440
	Private Sub LanguageUpdate(english As Boolean)
		For i As Integer = 0 To Me.recordEnglish.Length - 1
			Me.recordEnglish(i).SetActive(english)
		Next
		For j As Integer = 0 To Me.gollyEnglish.Length - 1
			Me.gollyEnglish(j).SetActive(english)
		Next
		For k As Integer = 0 To Me.recordOther.Length - 1
			Me.recordOther(k).SetActive(Not english)
		Next
		For l As Integer = 0 To Me.gollyOther.Length - 1
			Me.gollyOther(l).SetActive(Not english)
		Next
	End Sub

	' Token: 0x0600458A RID: 17802 RVA: 0x0024A0F0 File Offset: 0x002484F0
	Private Iterator Function fade_text_cr() As IEnumerator
		Dim t As Single = 0F
		Dim fadeTime As Single = 0.29F
		Dim topGradeLabelColor As Color = Me.topGradeLabel.color
		Dim topGradeValColor As Color = Me.topGradeValue.color
		While t < fadeTime
			t += CupheadTime.Delta
			Me.topGradeLabel.color = New Color(topGradeLabelColor.r, topGradeLabelColor.g, topGradeLabelColor.b, 1F - t / fadeTime)
			Me.topGradeValue.color = New Color(topGradeValColor.r, topGradeValColor.g, topGradeValColor.b, 1F - t / fadeTime)
			If Me.tryExpert.gameObject.activeSelf Then
				For Each spriteRenderer As SpriteRenderer In Me.tryExpert.GetComponentsInChildren(Of SpriteRenderer)()
					spriteRenderer.color = New Color(1F, 1F, 1F, 1F - t / fadeTime)
				Next
				For Each rawImage As RawImage In Me.tryExpert.GetComponentsInChildren(Of RawImage)()
					rawImage.color = New Color(1F, 1F, 1F, 1F - t / fadeTime)
				Next
				For Each textMeshCurveAndJitter As TextMeshCurveAndJitter In Me.tryExpert.GetComponentsInChildren(Of TextMeshCurveAndJitter)()
					Dim num As Single = Mathf.Clamp(255F - t / fadeTime * 255F, 0F, 255F)
					textMeshCurveAndJitter.AlphaValue = Convert.ToByte(num)
				Next
			End If
			If Me.tryRegular.gameObject.activeSelf Then
				For Each spriteRenderer2 As SpriteRenderer In Me.tryRegular.GetComponentsInChildren(Of SpriteRenderer)()
					spriteRenderer2.color = New Color(1F, 1F, 1F, 1F - t / fadeTime)
				Next
				For Each rawImage2 As RawImage In Me.tryRegular.GetComponentsInChildren(Of RawImage)()
					rawImage2.color = New Color(1F, 1F, 1F, 1F - t / fadeTime)
				Next
				For Each textMeshCurveAndJitter2 As TextMeshCurveAndJitter In Me.tryRegular.GetComponentsInChildren(Of TextMeshCurveAndJitter)()
					Dim num2 As Single = Mathf.Clamp(255F - t / fadeTime * 255F, 0F, 255F)
					textMeshCurveAndJitter2.AlphaValue = Convert.ToByte(num2)
				Next
			End If
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x04004B9B RID: 19355
	Private Shared NormalCurveValues As Integer() = New Integer() { 0, 38, 28, 65, 25, 36, 8, 28, 26, 40, 5, 5 }

	' Token: 0x04004B9C RID: 19356
	Private Shared NormalCurveOffsets As Integer() = New Integer() { 0, 21, 16, 37, 17, 23, 6, 17, 15, 22, 6, 6 }

	' Token: 0x04004B9D RID: 19357
	Private Shared GollyCurveValues As Integer() = New Integer() { 0, 53, 47, 51, 54, 54, 20, 51, 49, 49, 51, 26 }

	' Token: 0x04004B9E RID: 19358
	Private Shared GollyCurveOffsets As Integer() = New Integer() { 0, 30, 28, 31, 31, 31, 16, 29, 29, 29, 29, 16 }

	' Token: 0x04004B9F RID: 19359
	<SerializeField()>
	Private text As Text

	' Token: 0x04004BA0 RID: 19360
	<SerializeField()>
	Private topGradeLabel As TextMeshProUGUI

	' Token: 0x04004BA1 RID: 19361
	<SerializeField()>
	Private topGradeValue As Text

	' Token: 0x04004BA2 RID: 19362
	<SerializeField()>
	Private grades As String()

	' Token: 0x04004BA3 RID: 19363
	<SerializeField()>
	Private circle As Animator

	' Token: 0x04004BA4 RID: 19364
	<SerializeField()>
	Private recordBanner As Animator

	' Token: 0x04004BA5 RID: 19365
	<SerializeField()>
	Private recordEnglish As GameObject()

	' Token: 0x04004BA6 RID: 19366
	<SerializeField()>
	Private recordOther As GameObject()

	' Token: 0x04004BA7 RID: 19367
	<SerializeField()>
	Private recordBannerEnglish As Image

	' Token: 0x04004BA8 RID: 19368
	<SerializeField()>
	Private recordBannerOther As Image

	' Token: 0x04004BA9 RID: 19369
	<SerializeField()>
	Private gollyBanner As Animator

	' Token: 0x04004BAA RID: 19370
	<SerializeField()>
	Private gollyEnglish As GameObject()

	' Token: 0x04004BAB RID: 19371
	<SerializeField()>
	Private gollyOther As GameObject()

	' Token: 0x04004BAC RID: 19372
	<SerializeField()>
	Private gollyBannerEnglish As Image

	' Token: 0x04004BAD RID: 19373
	<SerializeField()>
	Private gollyBannerOther As Image

	' Token: 0x04004BAE RID: 19374
	<SerializeField()>
	Private tryRegular As SpriteRenderer

	' Token: 0x04004BAF RID: 19375
	<SerializeField()>
	Private tryExpert As SpriteRenderer

	' Token: 0x04004BB0 RID: 19376
	<SerializeField()>
	Private normalBannerTexts As GameObject()

	' Token: 0x04004BB1 RID: 19377
	<SerializeField()>
	Private topScoreBannerTexts As GameObject()

	' Token: 0x04004BB4 RID: 19380
	Private Const COUNTER_TIME As Single = 0.02F

	' Token: 0x04004BB5 RID: 19381
	Private Const BANNER_FLASH_Y_OFFSET As Single = 2F

	' Token: 0x04004BB8 RID: 19384
	Private input As CupheadInput.AnyPlayerInput
End Class
