Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.U2D
Imports UnityEngine.UI

' Token: 0x02000454 RID: 1108
Public Class AchievementToastManager
	Inherits AbstractMonoBehaviour

	' Token: 0x060010B7 RID: 4279 RVA: 0x000A057F File Offset: 0x0009E97F
	Private Sub OnEnable()
		AddHandler LocalAchievementsManager.AchievementUnlockedEvent, AddressOf Me.UnlockAchievement
	End Sub

	' Token: 0x060010B8 RID: 4280 RVA: 0x000A0592 File Offset: 0x0009E992
	Private Sub OnDisable()
		RemoveHandler LocalAchievementsManager.AchievementUnlockedEvent, AddressOf Me.UnlockAchievement
	End Sub

	' Token: 0x060010B9 RID: 4281 RVA: 0x000A05A8 File Offset: 0x0009E9A8
	Private Sub Start()
		Me.uiCamera = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.uiCameraPrefab)
		Me.uiCamera.transform.SetParent(MyBase.transform)
		Me.uiCamera.transform.ResetLocalTransforms()
		Dim component As Camera = Me.uiCamera.GetComponent(Of Camera)()
		component.cullingMask = 65536
		component.depth = CSng(AchievementToastManager.CameraDepth)
		Dim componentInChildren As Canvas = MyBase.GetComponentInChildren(Of Canvas)(True)
		componentInChildren.worldCamera = component
		componentInChildren.sortingLayerName = SpriteLayer.AchievementToast.ToString()
		Me.uiCamera.SetActive(False)
	End Sub

	' Token: 0x060010BA RID: 4282 RVA: 0x000A0640 File Offset: 0x0009EA40
	Public Sub UnlockAchievement(achievement As LocalAchievementsManager.Achievement)
		If Me.currentAnimation IsNot Nothing Then
			Me.queuedAchievements.Add(achievement)
			Return
		End If
		Me.currentAnimation = MyBase.StartCoroutine(Me.showUnlock(achievement))
	End Sub

	' Token: 0x060010BB RID: 4283 RVA: 0x000A0670 File Offset: 0x0009EA70
	Private Iterator Function showUnlock(achievement As LocalAchievementsManager.Achievement) As IEnumerator
		AudioManager.Play("achievement_unlocked")
		Dim achievementName As String = achievement.ToString()
		Dim titleKey As String = "Achievement" + achievementName + "Toast"
		Me.titleLocalization.ApplyTranslation(Localization.Find(titleKey), Nothing)
		Dim spriteName As String = achievementName + "_toast"
		Dim sprite As Sprite = Me.getAtlas(achievement).GetSprite(spriteName)
		Me.icon.sprite = sprite
		Me.toastTransform.position = AchievementToastManager.InitialPosition
		Me.visual.SetActive(True)
		Me.uiCamera.SetActive(True)
		Dim displacement As Vector2 = AchievementToastManager.FinalPosition - AchievementToastManager.InitialPosition
		Dim elapsed As Single = 0F
		While elapsed < AchievementToastManager.AnimationDuration
			elapsed += Time.unscaledDeltaTime
			Dim factor As Single = Me.easeOutBack(elapsed, 0F, 1F, AchievementToastManager.AnimationDuration)
			Me.toastTransform.localPosition = AchievementToastManager.InitialPosition + factor * displacement
			Yield Nothing
		End While
		Me.toastTransform.localPosition = AchievementToastManager.FinalPosition
		Yield New AchievementToastManager.WaitForSecondsRealtime(AchievementToastManager.HoldDuration)
		elapsed = 0F
		While elapsed < AchievementToastManager.AnimationDuration
			elapsed += Time.unscaledDeltaTime
			Dim factor2 As Single = Me.easeInBack(elapsed, 1F, -1F, AchievementToastManager.AnimationDuration)
			Me.toastTransform.localPosition = AchievementToastManager.InitialPosition + factor2 * displacement
			Yield Nothing
		End While
		If Me.queuedAchievements.Count > 0 Then
			Dim achievement2 As LocalAchievementsManager.Achievement = Me.queuedAchievements(0)
			Me.queuedAchievements.RemoveAt(0)
			Me.currentAnimation = MyBase.StartCoroutine(Me.showUnlock(achievement2))
		Else
			Me.currentAnimation = Nothing
			Me.visual.SetActive(False)
			Me.uiCamera.SetActive(False)
		End If
		Return
	End Function

	' Token: 0x060010BC RID: 4284 RVA: 0x000A0694 File Offset: 0x0009EA94
	Private Function easeOutBack(t As Single, initial As Single, change As Single, duration As Single) As Single
		Dim num As Single = 1.70158F
		Dim num2 As Single = t / duration - 1F
		t = num2
		Return change * (num2 * t * ((num + 1F) * t + num) + 1F) + initial
	End Function

	' Token: 0x060010BD RID: 4285 RVA: 0x000A06CC File Offset: 0x0009EACC
	Private Function easeInBack(t As Single, initial As Single, change As Single, duration As Single) As Single
		Dim num As Single = 1.70158F
		Dim num2 As Single = t / duration
		t = num2
		Dim num3 As Single = num2
		Return change * num3 * t * ((num + 1F) * t - num) + initial
	End Function

	' Token: 0x060010BE RID: 4286 RVA: 0x000A06FA File Offset: 0x0009EAFA
	Private Function getAtlas(achievement As LocalAchievementsManager.Achievement) As SpriteAtlas
		If Array.IndexOf(Of LocalAchievementsManager.Achievement)(LocalAchievementsManager.DLCAchievements, achievement) >= 0 Then
			Return Me.dlcAtlas
		End If
		Return Me.defaultAtlas
	End Function

	' Token: 0x170002A1 RID: 673
	' (get) Token: 0x060010BF RID: 4287 RVA: 0x000A071A File Offset: 0x0009EB1A
	Private ReadOnly Property defaultAtlas As SpriteAtlas
		Get
			If Not Me._defaultAtlasCached Then
				Me._defaultAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset("Achievements")
				Me._defaultAtlasCached = True
			End If
			Return Me._defaultAtlas
		End Get
	End Property

	' Token: 0x170002A2 RID: 674
	' (get) Token: 0x060010C0 RID: 4288 RVA: 0x000A0744 File Offset: 0x0009EB44
	Private ReadOnly Property dlcAtlas As SpriteAtlas
		Get
			If Not Me._dlcAtlasCached AndAlso DLCManager.DLCEnabled() Then
				Me._dlcAtlas = AssetLoader(Of SpriteAtlas).GetCachedAsset("Achievements_DLC")
				Me._dlcAtlasCached = True
			End If
			Return Me._dlcAtlas
		End Get
	End Property

	' Token: 0x040019FC RID: 6652
	Private Shared CameraDepth As Integer = 91

	' Token: 0x040019FD RID: 6653
	Private Shared InitialPosition As Vector2 = New Vector2(0F, -460F)

	' Token: 0x040019FE RID: 6654
	Private Shared FinalPosition As Vector2 = New Vector2(0F, -280F)

	' Token: 0x040019FF RID: 6655
	Public Shared AnimationDuration As Single = 0.34F

	' Token: 0x04001A00 RID: 6656
	Private Shared HoldDuration As Single = 2F

	' Token: 0x04001A01 RID: 6657
	<SerializeField()>
	Private uiCameraPrefab As GameObject

	' Token: 0x04001A02 RID: 6658
	<SerializeField()>
	Private visual As GameObject

	' Token: 0x04001A03 RID: 6659
	<SerializeField()>
	Private toastTransform As RectTransform

	' Token: 0x04001A04 RID: 6660
	<SerializeField()>
	Private titleLocalization As LocalizationHelper

	' Token: 0x04001A05 RID: 6661
	<SerializeField()>
	Private icon As Image

	' Token: 0x04001A06 RID: 6662
	Private queuedAchievements As List(Of LocalAchievementsManager.Achievement) = New List(Of LocalAchievementsManager.Achievement)()

	' Token: 0x04001A07 RID: 6663
	Private currentAnimation As Coroutine

	' Token: 0x04001A08 RID: 6664
	Private uiCamera As GameObject

	' Token: 0x04001A09 RID: 6665
	Private _defaultAtlasCached As Boolean

	' Token: 0x04001A0A RID: 6666
	Private _defaultAtlas As SpriteAtlas

	' Token: 0x04001A0B RID: 6667
	Private _dlcAtlasCached As Boolean

	' Token: 0x04001A0C RID: 6668
	Private _dlcAtlas As SpriteAtlas

	' Token: 0x02000455 RID: 1109
	Public Class WaitForSecondsRealtime
		Inherits CustomYieldInstruction

		' Token: 0x060010C2 RID: 4290 RVA: 0x000A07C8 File Offset: 0x0009EBC8
		Public Sub New(time As Single)
			Me.waitTime = time
		End Sub

		' Token: 0x170002A3 RID: 675
		' (get) Token: 0x060010C3 RID: 4291 RVA: 0x000A07E2 File Offset: 0x0009EBE2
		' (set) Token: 0x060010C4 RID: 4292 RVA: 0x000A07EA File Offset: 0x0009EBEA
		Public Property waitTime As Single

		' Token: 0x170002A4 RID: 676
		' (get) Token: 0x060010C5 RID: 4293 RVA: 0x000A07F4 File Offset: 0x0009EBF4
		Public Overrides ReadOnly Property keepWaiting As Boolean
			Get
				If Me.m_WaitUntilTime < 0F Then
					Me.m_WaitUntilTime = Time.realtimeSinceStartup + Me.waitTime
				End If
				Dim flag As Boolean = Time.realtimeSinceStartup < Me.m_WaitUntilTime
				If Not flag Then
					Me.m_WaitUntilTime = -1F
				End If
				Return flag
			End Get
		End Property

		' Token: 0x04001A0E RID: 6670
		Private m_WaitUntilTime As Single = -1F
	End Class
End Class
