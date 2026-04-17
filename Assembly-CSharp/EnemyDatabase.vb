Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000002 RID: 2
Public Class EnemyDatabase
	Inherits ScriptableObject

	' Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000458
	Public Shared Function GetProperties(id As EnemyID) As EnemyProperties
		If id = EnemyID.Undefined Then
			Return Nothing
		End If
		If id = EnemyID.blue_goblin Then
			Return EnemyDatabase.Instance.enemyProperties(0)
		End If
		If id = EnemyID.pink_goblin Then
			Return EnemyDatabase.Instance.enemyProperties(1)
		End If
		If id = EnemyID.blob_runner Then
			Return EnemyDatabase.Instance.enemyProperties(3)
		End If
		If id = EnemyID.lobber Then
			Return EnemyDatabase.Instance.enemyProperties(4)
		End If
		If id = EnemyID.flower_grunt Then
			Return EnemyDatabase.Instance.enemyProperties(5)
		End If
		If id = EnemyID.mushroom Then
			Return EnemyDatabase.Instance.enemyProperties(6)
		End If
		If id = EnemyID.chomper Then
			Return EnemyDatabase.Instance.enemyProperties(7)
		End If
		If id = EnemyID.acorn Then
			Return EnemyDatabase.Instance.enemyProperties(8)
		End If
		If id = EnemyID.spiker Then
			Return EnemyDatabase.Instance.enemyProperties(10)
		End If
		If id = EnemyID.miner Then
			Return EnemyDatabase.Instance.enemyProperties(28)
		End If
		If id = EnemyID.fan Then
			Return EnemyDatabase.Instance.enemyProperties(29)
		End If
		If id = EnemyID.wind Then
			Return EnemyDatabase.Instance.enemyProperties(2)
		End If
		If id = EnemyID.lobster Then
			Return EnemyDatabase.Instance.enemyProperties(16)
		End If
		If id = EnemyID.barnacle Then
			Return EnemyDatabase.Instance.enemyProperties(17)
		End If
		If id = EnemyID.urchin Then
			Return EnemyDatabase.Instance.enemyProperties(18)
		End If
		If id = EnemyID.crab Then
			Return EnemyDatabase.Instance.enemyProperties(19)
		End If
		If id = EnemyID.krill Then
			Return EnemyDatabase.Instance.enemyProperties(20)
		End If
		If id = EnemyID.clam Then
			Return EnemyDatabase.Instance.enemyProperties(21)
		End If
		If id = EnemyID.starfish Then
			Return EnemyDatabase.Instance.enemyProperties(22)
		End If
		If id = EnemyID.ladybug Then
			Return EnemyDatabase.Instance.enemyProperties(11)
		End If
		If id = EnemyID.dragonfly Then
			Return EnemyDatabase.Instance.enemyProperties(12)
		End If
		If id = EnemyID.woodpecker Then
			Return EnemyDatabase.Instance.enemyProperties(14)
		End If
		If id = EnemyID.acornmaker Then
			Return EnemyDatabase.Instance.enemyProperties(9)
		End If
		If id = EnemyID.beetle Then
			Return EnemyDatabase.Instance.enemyProperties(15)
		End If
		If id = EnemyID.dragonflyshot Then
			Return EnemyDatabase.Instance.enemyProperties(13)
		End If
		If id = EnemyID.flyingfish Then
			Return EnemyDatabase.Instance.enemyProperties(23)
		End If
		If id = EnemyID.satyr Then
			Return EnemyDatabase.Instance.enemyProperties(24)
		End If
		If id = EnemyID.mudman Then
			Return EnemyDatabase.Instance.enemyProperties(25)
		End If
		If id = EnemyID.smallmudman Then
			Return EnemyDatabase.Instance.enemyProperties(26)
		End If
		If id = EnemyID.dragon Then
			Return EnemyDatabase.Instance.enemyProperties(27)
		End If
		If id = EnemyID.wall Then
			Return EnemyDatabase.Instance.enemyProperties(31)
		End If
		If id = EnemyID.flamer Then
			Return EnemyDatabase.Instance.enemyProperties(30)
		End If
		If id = EnemyID.funhousewall Then
			Return EnemyDatabase.Instance.enemyProperties(32)
		End If
		If id = EnemyID.funwall2 Then
			Return EnemyDatabase.Instance.enemyProperties(33)
		End If
		If id = EnemyID.rocket Then
			Return EnemyDatabase.Instance.enemyProperties(34)
		End If
		If id = EnemyID.jack Then
			Return EnemyDatabase.Instance.enemyProperties(35)
		End If
		If id = EnemyID.duck Then
			Return EnemyDatabase.Instance.enemyProperties(36)
		End If
		If id = EnemyID.jackinbox Then
			Return EnemyDatabase.Instance.enemyProperties(38)
		End If
		If id = EnemyID.tuba Then
			Return EnemyDatabase.Instance.enemyProperties(39)
		End If
		If id = EnemyID.starcannon Then
			Return EnemyDatabase.Instance.enemyProperties(40)
		End If
		If id = EnemyID.miniduck Then
			Return EnemyDatabase.Instance.enemyProperties(37)
		End If
		If id = EnemyID.balloon Then
			Return EnemyDatabase.Instance.enemyProperties(41)
		End If
		If id = EnemyID.pretzel Then
			Return EnemyDatabase.Instance.enemyProperties(42)
		End If
		If id = EnemyID.arcade Then
			Return EnemyDatabase.Instance.enemyProperties(43)
		End If
		If id = EnemyID.ballrunner Then
			Return EnemyDatabase.Instance.enemyProperties(44)
		End If
		If id = EnemyID.magician Then
			Return EnemyDatabase.Instance.enemyProperties(45)
		End If
		If id = EnemyID.polebot Then
			Return EnemyDatabase.Instance.enemyProperties(46)
		End If
		If id = EnemyID.log Then
			Return EnemyDatabase.Instance.enemyProperties(47)
		End If
		If id <> EnemyID.hotdog Then
			Return EnemyDatabase.defaultProperties
		End If
		Return EnemyDatabase.Instance.enemyProperties(48)
	End Function

	' Token: 0x17000001 RID: 1
	' (get) Token: 0x06000003 RID: 3 RVA: 0x000025FB File Offset: 0x000009FB
	Public Shared ReadOnly Property Instance As EnemyDatabase
		Get
			If EnemyDatabase._instance Is Nothing Then
				EnemyDatabase._instance = Resources.Load(Of EnemyDatabase)("EnemyDatabase/data_enemies")
			End If
			Return EnemyDatabase._instance
		End Get
	End Property

	' Token: 0x04000001 RID: 1
	Private Shared defaultProperties As EnemyProperties = New EnemyProperties()

	' Token: 0x04000002 RID: 2
	Public Const PATH As String = "EnemyDatabase/data_enemies"

	' Token: 0x04000003 RID: 3
	Private Shared _instance As EnemyDatabase

	' Token: 0x04000004 RID: 4
	Public enemyProperties As List(Of EnemyProperties)

	' Token: 0x04000005 RID: 5
	Public index As Integer
End Class
