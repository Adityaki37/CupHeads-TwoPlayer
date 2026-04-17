Imports System
Imports Rewired
Imports UnityEngine

' Token: 0x020009B5 RID: 2485
Public Class StartScreenAudio
	Inherits AbstractMonoBehaviour

	' Token: 0x170004BA RID: 1210
	' (get) Token: 0x06003A45 RID: 14917 RVA: 0x00211C84 File Offset: 0x00210084
	Public Shared ReadOnly Property Instance As StartScreenAudio
		Get
			Return StartScreenAudio.startScreenAudio
		End Get
	End Property

	' Token: 0x06003A46 RID: 14918 RVA: 0x00211C8B File Offset: 0x0021008B
	Private Sub Start()
		Me.blockInput = CreditsScreen.goodEnding
		Me.players = New Player() { PlayerManager.GetPlayerInput(PlayerId.PlayerOne), PlayerManager.GetPlayerInput(PlayerId.PlayerTwo) }
	End Sub

	' Token: 0x06003A47 RID: 14919 RVA: 0x00211CB8 File Offset: 0x002100B8
	Private Sub Update()
		If Me.blockInput Then
			Return
		End If
		If Me.codeIndex < Me.code.Length Then
			For Each player As Player In Me.players
				If player.GetAnyButtonDown() Then
					If player.GetButtonDown(CInt(Me.code(Me.codeIndex))) Then
						Me.codeIndex += 1
					ElseIf Not player.GetButtonDown(CInt(Me.code(Me.codeIndex))) Then
						Me.codeIndex = 0
					End If
				End If
			Next
		Else
			If Me.bgmAlt2.clip Is Nothing Then
				Me.bgmAlt2.GetComponent(Of DeferredAudioSource)().Initialize()
			End If
			AudioManager.StopBGM()
			Me.bgmAlt2.Play()
			Me.blockInput = True
		End If
	End Sub

	' Token: 0x06003A48 RID: 14920 RVA: 0x00211D9A File Offset: 0x0021019A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		StartScreenAudio.startScreenAudio = Me
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x0400427B RID: 17019
	<SerializeField()>
	Private bgmAlt2 As AudioSource

	' Token: 0x0400427C RID: 17020
	Private Shared startScreenAudio As StartScreenAudio

	' Token: 0x0400427D RID: 17021
	Private code As CupheadButton() = New CupheadButton() { CupheadButton.MenuUp, CupheadButton.MenuUp, CupheadButton.MenuDown, CupheadButton.MenuDown, CupheadButton.MenuLeft, CupheadButton.MenuRight, CupheadButton.MenuLeft, CupheadButton.MenuRight, CupheadButton.Cancel, CupheadButton.Accept }

	' Token: 0x0400427E RID: 17022
	Private codeIndex As Integer

	' Token: 0x0400427F RID: 17023
	Private players As Player()

	' Token: 0x04004280 RID: 17024
	Private blockInput As Boolean
End Class
