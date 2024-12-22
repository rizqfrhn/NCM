-- Script Date: 12/22/2024 9:19 PM  - ErikEJ.SqlCeScripting version 3.5.2.95
DELETE FROM tb_bridging;
INSERT INTO [tb_bridging]
           ([Id_bridging]
           ,[desc])
     VALUES
           ('l25nat', 'ARPNAT (pseudo L2 NAT)'),
           ('wds', '4 addresses format (WDS)'),
           ('mono_clone', 'Wired device cloning (only one)'),
           ('mono_profinet', 'Profinet device cloning (only one)');
