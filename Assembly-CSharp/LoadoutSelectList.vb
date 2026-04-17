Imports System
Imports System.Collections
Imports RektTransform
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020009A8 RID: 2472
Public Class LoadoutSelectList
	Inherits AbstractMonoBehaviour

	' Token: 0x060039FE RID: 14846 RVA: 0x0020F4A2 File Offset: 0x0020D8A2
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.SetupList()
	End Sub

	' Token: 0x060039FF RID: 14847 RVA: 0x0020F4B0 File Offset: 0x0020D8B0
	Private Sub SetupList()
		Dim array As Array = Nothing
		Select Case Me.mode
			Case LoadoutSelectList.Mode.Primary, LoadoutSelectList.Mode.Secondary
				array = [Enum].GetValues(GetType(Weapon))
			Case LoadoutSelectList.Mode.Super
				array = [Enum].GetValues(GetType(Super))
			Case LoadoutSelectList.Mode.Charm
				array = [Enum].GetValues(GetType(Charm))
			Case LoadoutSelectList.Mode.Difficulty
				array = [Enum].GetValues(GetType(Level.Mode))
		End Select
		Dim num As Integer = 0
		Dim enumerator As IEnumerator = array.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim <SetupList>c__AnonStorey As LoadoutSelectList.<SetupList>c__AnonStorey0 = New LoadoutSelectList.<SetupList>c__AnonStorey0()
				<SetupList>c__AnonStorey.value = enumerator.Current
				<SetupList>c__AnonStorey.$this = Me
				Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.button.gameObject)
				Dim b As Button = gameObject.GetComponent(Of Button)()
				Dim text As String = <SetupList>c__AnonStorey.value.ToString().Replace("level_", String.Empty).Replace("weapon_", String.Empty).Replace("super_", String.Empty).Replace("charm_", String.Empty)
				b.name = <SetupList>c__AnonStorey.value.ToString()
				gameObject.GetComponentInChildren(Of Text)().text = text
				b.onClick.AddListener(Sub()
					Dim array2 As PlayerId() = New PlayerId() { PlayerId.PlayerOne, PlayerId.PlayerTwo }
					For Each playerId As PlayerId In array2
						Dim playerLoadout As PlayerData.PlayerLoadouts.PlayerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId)
						Select Case <SetupList>c__AnonStorey.$this.mode
							Case LoadoutSelectList.Mode.Primary
								playerLoadout.primaryWeapon = CType(<SetupList>c__AnonStorey.value, Weapon)
							Case LoadoutSelectList.Mode.Secondary
								playerLoadout.secondaryWeapon = CType(<SetupList>c__AnonStorey.value, Weapon)
							Case LoadoutSelectList.Mode.Super
								playerLoadout.super = CType(<SetupList>c__AnonStorey.value, Super)
							Case LoadoutSelectList.Mode.Charm
								Dim charm As Charm = CType(<SetupList>c__AnonStorey.value, Charm)
								playerLoadout.charm = charm
							Case LoadoutSelectList.Mode.Difficulty
								Level.SetCurrentMode(CType(<SetupList>c__AnonStorey.value, Level.Mode))
						End Select
					Next
					If <SetupList>c__AnonStorey.$this.selectedButton IsNot Nothing Then
						Dim colors As ColorBlock = <SetupList>c__AnonStorey.$this.selectedButton.colors
						colors.colorMultiplier = 2F
						<SetupList>c__AnonStorey.$this.selectedButton.colors = colors
					End If
					<SetupList>c__AnonStorey.$this.selectedButton = b
					Dim colors2 As ColorBlock = <SetupList>c__AnonStorey.$this.selectedButton.colors
					colors2.colorMultiplier = 4F
					<SetupList>c__AnonStorey.$this.selectedButton.colors = colors2
				End Sub)
				b.transform.SetParent(Me.button.transform.parent)
				b.transform.ResetLocalTransforms()
				num += 1
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			If disposable IsNot Nothing Then
				disposable2.Dispose()
			End If
		End Try
		Me.button.gameObject.SetActive(False)
		Me.contentPanel.SetHeight(30F * CSng(num))
	End Sub

	' Token: 0x040041E1 RID: 16865
	<SerializeField()>
	Public mode As LoadoutSelectList.Mode

	' Token: 0x040041E2 RID: 16866
	Public button As Button

	' Token: 0x040041E3 RID: 16867
	Public selectedButton As Button

	' Token: 0x040041E4 RID: 16868
	Public contentPanel As RectTransform

	' Token: 0x020009A9 RID: 2473
	Public Enum Mode
		' Token: 0x040041E6 RID: 16870
		Primary
		' Token: 0x040041E7 RID: 16871
		Secondary
		' Token: 0x040041E8 RID: 16872
		Super
		' Token: 0x040041E9 RID: 16873
		Charm
		' Token: 0x040041EA RID: 16874
		Difficulty
	End Enum
End Class
