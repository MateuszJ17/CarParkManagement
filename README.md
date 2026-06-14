<h1>Car Park Management</h1>
<br/>
<h2>How to run the solution:</h2>
<br/>
<h3>1. Running DB in docker, api locally:</h3>
<ul>
<li>Run docker-compose up postgres</li>
<li>Run the api using your preffered method (dotnet run, IDE, etc.)</li>
<br/>
<span>Migrations should be applied automatically. You migth see a first time relation '__EFMigrationsHistory' does not exist
error but as far as I know it is normal in the first time setup. Ignore it, DB should get relevant tables and parking spaces
seed data.</span>
</ul>
<br/>
<h3>2. Running all in docker:</h3>
<ul>
<li>Run docker-compose up</li>
</ul>
<br/>
<span>Migrations should be applied automatically. You migth see a first time relation '__EFMigrationsHistory' does not exist
error but as far as I know it is normal in the first time setup. Ignore it, DB should get relevant tables and parking spaces
seed data. <b>Please keep in mind that the ports will be different and that only http is supported!</b></span>
<br/>
<h2>Assumptions made:</h2>
<ul>
<li>I've assumed that the parking has only one enter and cars enter in a one after another manner. This prevents
me having to deal with a race condition where two cars would enter at the same time (or very close to it).
If the parking would accept multiple cars at once then we would have to solve it, for example by using
concurrency tokens in EF.</li>
<li>Parking space assignemt uses a simplistic algorith that basically says "always sort the spaces the same way
and assign first available space". In real life maybe we would like to redistribute traffic in another way, something like
two pointers where first pointer points at the start of the parking and the other at the end of the parking.</li>
<li>Charging is done on the assumption of if a minute started we charge for it, but for the five minutes extra charge
we only add the charge when the full five minutes have passed.</li>
<li>I've assumed that 'exit' means car leaving and paying at the barrier.</li>
</ul>
<br/>
<h2>Questions I'd ask:</h2>
<ul>
<li>Do we need to support entering multiple cars at once</li>
<li>Do we want any particular way to assign free parking spaces or do we have a free hand in deciding?</li>
<li>Do we have any limits on how long cars can stay? Any rules to prevent abandoning cars on our parking?</li>
<li>Chargin rules. Do we want to charge when the minute starts? Do we want to add the extra charge after
full five minutes pass or if the next five minut block starts?</li>
<li>Are we charge at the literal exit or do we have some kind of payment machines and do we need to add a x minute
grace period for the car to exit the parking?</li>
</ul>