Imports System
Imports UnityEngine
Imports UnityEngine.SceneManagement
Imports UnityEngine.UI

' Token: 0x020009B0 RID: 2480
Public Class UserProfileDisplay
	Inherits AbstractMonoBehaviour

	' Token: 0x06003A2C RID: 14892 RVA: 0x002113A4 File Offset: 0x0020F7A4
	Protected Overrides Sub Awake()
		Me.root.SetActive(False)
		Me.gamerPic.gameObject.SetActive(False)
	End Sub

	' Token: 0x06003A2D RID: 14893 RVA: 0x002113C4 File Offset: 0x0020F7C4
	Private Sub Start()
		Me.isSlotSelect = SceneManager.GetActiveScene().name = Scenes.scene_slot_select.ToString()
	End Sub

	' Token: 0x06003A2E RID: 14894 RVA: 0x002113F8 File Offset: 0x0020F7F8
	Private Sub Update()
		If OnlineManager.Instance.[Interface].SupportsMultipleUsers OrElse (Me.showForMultipleUsersUnsupported AndAlso OnlineManager.Instance.[Interface].SupportsUserSignIn) Then
			Dim user As OnlineUser = OnlineManager.Instance.[Interface].GetUser(Me.player)
			If user IsNot Nothing Then
				Me.root.SetActive(True)
				Dim name As String = user.Name
				If Me.gamerTag.text <> name Then
					Me.gamerTag.text = name
				End If
				Dim profilePic As Texture2D = OnlineManager.Instance.[Interface].GetProfilePic(Me.player)
				If profilePic IsNot Nothing AndAlso Me.currentPicUser IsNot user Then
					Me.currentPicUser = user
					Dim sprite As Sprite = Sprite.Create(profilePic, New Rect(0F, 0F, CSng(profilePic.width), CSng(profilePic.height)), New Vector2(0.5F, 0.5F))
					Me.gamerPic.sprite = sprite
					Me.gamerPic.gameObject.SetActive(True)
				ElseIf profilePic Is Nothing Then
					Me.gamerPic.gameObject.SetActive(False)
				End If
			Else
				Me.root.SetActive(False)
			End If
		Else
			Me.root.SetActive(False)
		End If
	End Sub

	' Token: 0x0400425E RID: 16990
	<SerializeField()>
	Private gamerPic As Image

	' Token: 0x0400425F RID: 16991
	<SerializeField()>
	Private gamerTag As Text

	' Token: 0x04004260 RID: 16992
	<SerializeField()>
	Private player As PlayerId

	' Token: 0x04004261 RID: 16993
	<SerializeField()>
	Private root As GameObject

	' Token: 0x04004262 RID: 16994
	<SerializeField()>
	Private switchPromptRoot As GameObject

	' Token: 0x04004263 RID: 16995
	<SerializeField()>
	Private showForMultipleUsersUnsupported As Boolean

	' Token: 0x04004264 RID: 16996
	<SerializeField()>
	Private defaultAvatarCuphead As Sprite

	' Token: 0x04004265 RID: 16997
	<SerializeField()>
	Private defaultAvatarMugman As Sprite

	' Token: 0x04004266 RID: 16998
	Private currentPicUser As OnlineUser

	' Token: 0x04004267 RID: 16999
	Private isSlotSelect As Boolean
End Class
