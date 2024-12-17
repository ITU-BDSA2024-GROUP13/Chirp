<!-- 
Illustrate with a UML activity diagram how your Chirp! applications are build, tested, released, and deployed. That is, illustrate the flow of activities in your respective GitHub Actions workflows.

Describe the illustration briefly, i.e., how your application is built, tested, released, and deployed.
 -->

<style>
.uml-body{
    display: flex;
    text-align: start;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 20px;
}

.uml-activity{
    display: flex;
    flex-direction: column;
    gap: 20px;
    font-size: larger;
}
</style>


<body>
<div class="uml-body">
<div class="uml-activity">

# UML Activity Diagram

<img alt="UML activity diagram" src="../../diagrams/yml.png" width="1000">
<span>
A key activity of this project has been on automating mundane tasks, which not only decreases the accumilated workload but also decreases the work process.

Through the use of github actions, the total amount of time spent manually creating releases, making dll's and deploying the service to azure has been reduced to nil hours - excluding the time creating the actions themselves.

Although introducing playwright testing to the project, caused some issues on github, and therefore testing in workflows was skipped. Maintaining the workflows and ensuring they work, has been a higher priority rather than the code-quality as using git allows for roll-backs.

The primary event that triggers the `Create Release` workflow is on an accepted pull-request and the secondary event is on a Sunday at 8:00 o'clock.

Once `Create release` has completed it triggers two other workflows, namely `Make dll` and `Build and deploy` respectively.
</span>
</div>

<div class="uml-activity">

### Creating a release

<img alt="UML activity diagram" src="../../diagrams/createRelease.png" width="1000">

<span>

As previously mentioned, the `Create release` workflow triggers on either a push to main or on a Sunday.  
The purpose of this workflow has been to create a release where the version-tag gets bumped based on the commit message.  
It follows the template of ***Major.Minor.Patch***:  

**Major**: a total rework of the system - such as switching from CLI to a web-based service.  
**Minor**: a new feature added to the current version of the system.  
**Patch**: bug fixes / formatting / refactors

The commit-message will get scanned for the keywords *Major* or *Minor*, otherwise it will default to a patch. 

This workflow also previously contained a testing step, but was removed.

</span>
</div>

<div class="uml-activity">

### Making dlls

<img alt="UML activity diagram" src="../../diagrams/makeDLL.png" width="1000">
<span>

`Make dll` will build the program, and create a zip-file containing the dlls.  
The worflow uses a matrix, that optimizes a certain part of the process, specifically *Process for creating a zip-file with .dll*, to reduce code-redundency. The main concern for this is supporting multiple OS-systems - and if needed, ease of adding a new OS-system.

Lastly all the files will the appended to the latest release, created by the `Create Release` workflow.

Therefore is it crucial that the prior step to the workflow is working, as if a new release doesn't get created, `Make dll` will replace the files from the newest release.

</span>
</div>

<div class="uml-activity">

### Deploying to production

<img alt="UML activity diagram" src="../../diagrams/BuildAndDeploy.png" width="1000">
<span>

The base template for `Build and deploy` has been created by Azure, where it was modified to wait upon `Create Release` for the confirmation of the aforementioned *test-step*, which has been deleted.

</span>
</div>


</div>
</body>

