/*
INSERTING MISSING PAYERS IN CCIS FROM LMU 
*/


insert into CCIS..Payers (PayerCode, PayerName,IsActive,PayerTypeID,CreationDate)
select  [DHA Payer ID],[Payer Name]
,(
case  [active]
when   'true' then 1 
when 'false' then 0 
end 
) as isActive
,1,GETDATE() from FS_WS_WSCTFW..LMUMasterPlans
where [DHA Payer ID] not in (
select payercode from ccis..Payers 
)

insert into CCIS..Payers (PayerCode, PayerName,IsActive,PayerTypeID,CreationDate)
select [HAAD Payer ID],[Payer Name]
,(
case  [active]
when   'true' then 1 
when 'false' then 0 
end 
) as isActive
,1,GETDATE() from FS_WS_WSCTFW..LMUMasterPlans
where [HAAD Payer ID] not in (
select payercode from ccis..Payers
)


insert into CCIS..Payers (PayerCode, PayerName,IsActive,PayerTypeID,CreationDate)
select [DHA Receiver ID],[Receiver Name]
,(
case  [active]
when   'true' then 1 
when 'false' then 0 
end 
) as isActive
,1,GETDATE() from FS_WS_WSCTFW..LMUMasterPlans
where [DHA Receiver ID] not in (
select payercode from ccis..Payers
)


insert into CCIS..Payers (PayerCode, PayerName,IsActive,PayerTypeID,CreationDate)
select [DHA Receiver ID],[Receiver Name]
,(
case  [active]
when   'true' then 1 
when 'false' then 0 
end 
) as isActive
,1,GETDATE() from FS_WS_WSCTFW..LMUMasterPlans
where rtrim(ltrim([DHA Receiver ID])) like '%TPA%'




insert into CCIS..Payers (PayerCode, PayerName,IsActive,PayerTypeID,CreationDate)
select [HAAD Receiver ID],[Receiver Name]
,(
case  [active]
when   'true' then 1 
when 'false' then 0 
end 
) as isActive
,1,GETDATE() from FS_WS_WSCTFW..LMUMasterPlans
where [HAAD Receiver ID] not in (
select payercode from ccis..Payers
)


