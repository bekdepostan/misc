select CombineStr,count(*) from combines group by combineStr
order by count(*) desc