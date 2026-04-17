Imports System
Imports System.Collections.Generic

' Token: 0x02000A43 RID: 2627
Public Module CharmCurse
	' Token: 0x06003EA2 RID: 16034 RVA: 0x00225B98 File Offset: 0x00223F98
	Public Function CalculateLevel(playerId As PlayerId) As Integer
		If Not PlayerData.Data.GetLevelData(Levels.Graveyard).completed Then
			Return -1
		End If
		Dim num As Integer = PlayerData.Data.CalculateCurseCharmAccumulatedValue(playerId, CharmCurse.CountableLevels)
		Dim levelThreshold As Integer() = WeaponProperties.CharmCurse.levelThreshold
		For i As Integer = 0 To levelThreshold.Length - 1
			If num < levelThreshold(i) Then
				Return i - 1
			End If
		Next
		Return levelThreshold.Length - 1
	End Function

	' Token: 0x06003EA3 RID: 16035 RVA: 0x00225C00 File Offset: 0x00224000
	Public Function IsMaxLevel(playerId As PlayerId) As Boolean
		Dim levelThreshold As Integer() = WeaponProperties.CharmCurse.levelThreshold
		Return CharmCurse.CalculateLevel(playerId) = levelThreshold.Length - 1
	End Function

	' Token: 0x06003EA4 RID: 16036 RVA: 0x00225C20 File Offset: 0x00224020
	Public Function GetHealthModifier(charmLevel As Integer) As Integer
		If charmLevel < 0 Then
			Return 0
		End If
		Return WeaponProperties.CharmCurse.healthModifierValues(charmLevel)
	End Function

	' Token: 0x06003EA5 RID: 16037 RVA: 0x00225C32 File Offset: 0x00224032
	Public Function GetSuperMeterAmount(charmLevel As Integer) As Single
		If charmLevel < 0 Then
			Return 0F
		End If
		Return WeaponProperties.CharmCurse.superMeterAmount(charmLevel)
	End Function

	' Token: 0x06003EA6 RID: 16038 RVA: 0x00225C48 File Offset: 0x00224048
	Public Function GetSmokeDashInterval(charmLevel As Integer) As Integer
		If charmLevel < 0 Then
			Return 0
		End If
		Return WeaponProperties.CharmCurse.smokeDashInterval(charmLevel)
	End Function

	' Token: 0x06003EA7 RID: 16039 RVA: 0x00225C5A File Offset: 0x0022405A
	Public Function GetWhetstoneInterval(charmLevel As Integer) As Integer
		If charmLevel < 0 Then
			Return 0
		End If
		Return WeaponProperties.CharmCurse.whetstoneInterval(charmLevel)
	End Function

	' Token: 0x06003EA8 RID: 16040 RVA: 0x00225C6C File Offset: 0x0022406C
	Public Function GetHealerInterval(charmLevel As Integer, hpReceived As Integer) As Integer
		If charmLevel < 0 Then
			Return 0
		End If
		If CharmCurse.healerCharmIntervals Is Nothing Then
			Dim healerInterval As String() = WeaponProperties.CharmCurse.healerInterval
			CharmCurse.healerCharmIntervals = New List(Of Integer())(healerInterval.Length)
			For Each text As String In healerInterval
				Dim array2 As String() = text.Split(New Char() { ","c })
				If array2.Length <> 3 Then
					Throw New Exception("Invalid healer intervals")
				End If
				Dim array3 As Integer() = New Integer(array2.Length - 1) {}
				For j As Integer = 0 To array3.Length - 1
					array3(j) = Parser.IntParse(array2(j))
				Next
				CharmCurse.healerCharmIntervals.Add(array3)
			Next
		End If
		Return CharmCurse.healerCharmIntervals(charmLevel)(hpReceived)
	End Function

	' Token: 0x040045B1 RID: 17841
	Public CountableLevels As Levels() = New Levels() { Levels.Veggies, Levels.Slime, Levels.FlyingBlimp, Levels.Flower, Levels.Frogs, Levels.Baroness, Levels.Clown, Levels.FlyingGenie, Levels.Dragon, Levels.FlyingBird, Levels.Bee, Levels.Pirate, Levels.SallyStagePlay, Levels.Mouse, Levels.Robot, Levels.FlyingMermaid, Levels.Train, Levels.DicePalaceBooze, Levels.DicePalaceChips, Levels.DicePalaceCigar, Levels.DicePalaceDomino, Levels.DicePalaceEightBall, Levels.DicePalaceFlyingHorse, Levels.DicePalaceFlyingMemory, Levels.DicePalaceRabbit, Levels.DicePalaceRoulette, Levels.DicePalaceMain, Levels.Devil, Levels.Airplane, Levels.RumRunners, Levels.OldMan, Levels.SnowCult, Levels.FlyingCowboy, Levels.Saltbaker }

	' Token: 0x040045B2 RID: 17842
	Private healerCharmIntervals As List(Of Integer())
End Module
